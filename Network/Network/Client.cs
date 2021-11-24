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
using Network.Database;

using Network.DataTransfer.Request;
using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace Network {

    public class Client {
        private Client() {
            
        }

        public static Client Instance {
            get { return _Instance ??= new Client(); }
            private set { _Instance = value; }
        }
        private static Client _Instance;


        public void Connect(string ip, int port) {
            Client.Data.Server = new TcpClient();

            Client.Data.Server.Connect(ip, port);
            Client.Data.Stream = Client.Data.Server.GetStream();

            if (Client.Data.Server.Connected) {
                StartListenerThread();
            }
        }

        private void StartListenerThread() {
            var received_data_queue = new BlockingCollection<object>();
            var cancellation = new CancellationTokenSource();

            // Receive data
            Task.Run(() => {
                byte[] receive_buffer = new byte[4 * 1024 * 1024]; // 4 MB cache
                int buffer_length = 0;

                while (true) {
                    try {
                        byte[] received_bytes = new byte[Client.Data.Server.Available];
                        Client.Data.Stream.Read(received_bytes, 0, received_bytes.Length);

                        Array.Copy(received_bytes, 0, receive_buffer, buffer_length, received_bytes.Length);
                        buffer_length += received_bytes.Length;

                        // TODO: Replace sleep with better solution
                        Thread.Sleep(1);

                        if (received_bytes.Length == 0 || Client.Data.Stream.DataAvailable == true) {
                            continue;
                        }

                        for (int index = 0, length = 0; index < buffer_length; index += (length + 4)) {
                            length = BitConverter.ToInt32(receive_buffer, index);

                            var data = new byte[length];
                            Array.Copy(receive_buffer, index + 4, data, 0, length);

                            var obj = Serializer.Deserialize(data);
                            received_data_queue.Add(obj);
                        }
                    }
                    catch (Exception e) {
                        if(Client.Data.Server.Connected == false) {
                            Console.WriteLine("Disconnected from the server!");
                            cancellation.Cancel();

                            break;
                        }

                        Console.WriteLine(e);
                    }

                    buffer_length = 0;
                }
            });

            // Process data
            Task.Run(() => {
                while (true) {
                    try {
                        var received_data = received_data_queue.Take(cancellation.Token);

                        switch (received_data) {
                            case BaseResponse: {
                                ResponseReceived?.Invoke(this, received_data as BaseResponse);
                                break;
                            }
                            case BaseNotification: {
                                NotificationReceived?.Invoke(this, received_data as BaseNotification);
                                break;
                            }
                        }
                    }
                    catch (Exception e) {
                        if(e is OperationCanceledException) {
                            break;
                        }

                        Console.WriteLine(e);
                    }
                }
            });
        }


        public void SendRequest(BaseRequest request) {
            try {
                lock (Client.Data.Stream) {
                    byte[] request_bytes = Serializer.Serialize(request);
                    Client.Data.Stream.Write(request_bytes, 0, request_bytes.Length);
                }
            }
            catch { }
        }


        public class Data {
            // User data (received from server)
            public static string UserID { get; set; }

            // Connection data
            internal static TcpClient Server { get; set; }
            internal static NetworkStream Stream { get; set; }
        }

        
        public event EventHandler<BaseResponse> ResponseReceived;
        public event EventHandler<BaseNotification> NotificationReceived;
    }

}