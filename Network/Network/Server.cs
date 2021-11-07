using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Network.Core;
using Network.DataTransfer.Request;
using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

// TODO: Remove this when database is implemented
using UserLoginDetails = System.Tuple<string, string, string>; // Tuple<Login, Password, UserID>

namespace Network {

    public class Server {
        private Server() {
            Console.SetOut(new TimestampTextWriter());
        }

        public static Server Instance {
            get { return _Instance ??= new Server(); }
            private set { _Instance = value; }
        }
        private static Server _Instance;


        public void Start(string ip_address, int port) {
            // Init server
            var ip = IPAddress.Parse(ip_address);

            Server.Data.Listener = new TcpListener(ip, port);
            Server.Data.ConnectedClients = new ThreadSafeList<ConnectedClient>();

            // Start server
            Server.Data.Listener.Start();

            Console.WriteLine("Server has started on {0}!", Server.Data.Listener.LocalEndpoint);
            Console.WriteLine("Waiting for connection...");

            while (true) {
                var client = Server.Data.Listener.AcceptTcpClient();
                StartClientThread(client);
            }
        }

        private void StartClientThread(TcpClient client) {
            var request_queue = new BlockingCollection<BaseRequest>();
            var cancellation = new CancellationTokenSource();
            var stream = client.GetStream();

            // Receive request
            Task.Run(() => {
                while (true) {
                    try {
                        byte[] request_bytes = new byte[client.Available];
                        stream.Read(request_bytes, 0, request_bytes.Length);

                        if (request_bytes.Length == 0) {
                            continue;
                        }

                        for (int index = 0, length = 0; index < request_bytes.Length; index += (length + 4)) {
                            length = BitConverter.ToInt32(request_bytes, index);

                            var data = new byte[length];
                            Array.Copy(request_bytes, index + 4, data, 0, length);

                            var request = Serializer.Deserialize(data) as BaseRequest;
                            request_queue.Add(request);
                        }
                    }
                    catch (Exception e) {
                        if(client.Connected == false) {
                            var connected_client = Server.Data.ConnectedClients.Find(p => (p.Client == client));

                            if (connected_client != null) {
                                Console.WriteLine("{0} connection lost!", connected_client.UserID);
                                Server.Data.ConnectedClients.Remove(connected_client);
                            }

                            cancellation.Cancel();
                            break;
                        }

                        Console.WriteLine(e);
                    }
                }
            });

            // Process request
            Task.Run(() => {
                while (true) {
                    try {
                        var request = request_queue.Take(cancellation.Token);
                        RequestResult result = null;

                        // Check if logged in
                        if (Server.Data.ConnectedClients.Find(p => (p.Client == client)) == null) {
                            if (request is LoginRequest) {
                                result = request.Process(client);
                            }
                            else {
                                continue;
                            }
                        }
                        else {
                            result = request.Process(client);
                        }

                        // Send response
                        if (result.ResponseReceiver != null && result.ResponseData != null) {
                            Server.SendResponse(result.ResponseReceiver, result.ResponseData);
                        }

                        // Send notifications
                        if (result.NotificationReceivers != null && result.NotificationData != null) {

                            foreach (var notification_receiver in result.NotificationReceivers) {
                                Server.SendNotification(notification_receiver, result.NotificationData);
                            }

                        }
                    }
                    catch (Exception e) {
                        if (e is OperationCanceledException) {
                            break;
                        }

                        Console.WriteLine(e);
                    }
                }
            });
        }


        internal static void SendResponse(TcpClient client, BaseResponse response) {
            try {
                var stream = client.GetStream();

                lock (stream) {
                    byte[] response_bytes = Serializer.Serialize(response);
                    stream.Write(response_bytes, 0, response_bytes.Length);
                }
            }
            catch {

            }
        }

        internal static void SendNotification(TcpClient client, BaseNotification notification) {
            try {
                var stream = client.GetStream();

                lock (stream) {
                    byte[] notification_bytes = Serializer.Serialize(notification);
                    stream.Write(notification_bytes, 0, notification_bytes.Length);
                }
            }
            catch { 
            
            }
        }


        internal class Data {
            public static TcpListener Listener { get; set; }
            public static ThreadSafeList<ConnectedClient> ConnectedClients { get; set; }

            // TODO: Replace this with database
            public static List<UserLoginDetails> UserLoginData = new List<UserLoginDetails> {
                new UserLoginDetails("Andrzej", "Password1", "Andrzej#0001"),
                new UserLoginDetails("Kasztan", "Password2", "Kasztan#0002"),
                new UserLoginDetails("Mariusz", "Password3", "Mariusz#0003")
            };
        }
    }


    internal class ConnectedClient {
        // User data
        public string UserID { get; set; }

        // Connection data
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }
    }

}