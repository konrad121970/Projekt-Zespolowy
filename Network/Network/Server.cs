using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network {

    class ConnectedClient {
        public string ID { get; set; }
        public NetworkStream Stream { get; set; }
    }

    public class Server {
        public Server(string ip_address, int port) {
            var ip = IPAddress.Parse(ip_address);

            _Server = new TcpListener(ip, port);
            _Clients = new List<ConnectedClient>();
        }

        public void Start() {
            Console.WriteLine("{0}: Server has started on {1}!", DateTime.Now.ToString("HH:mm:ss"), _Server.LocalEndpoint);
            Console.WriteLine("{0}: Waiting for connection...", DateTime.Now.ToString("HH:mm:ss"));

            _Server.Start();

            while (true) {
                var client = _Server.AcceptTcpClient();
                Task.Run(() => ClientThread(client));
            }
        }


        private void ClientThread(TcpClient tcp_client) {
            var client = new ConnectedClient();
            var stream = tcp_client.GetStream();

            while (true) {
                // Get Request
                Byte[] request_bytes = new Byte[tcp_client.Available];
                stream.Read(request_bytes, 0, request_bytes.Length);

                if (request_bytes.Length == 0) {
                    continue;
                }

                // Process Request
                string request = Encoding.UTF8.GetString(request_bytes);
                string request_type = request.Substring(0, request.IndexOf(':'));
                string request_data = request.Substring(request.IndexOf(':') + 2);

                switch (request_type) {
                    case "Login": {
                        client.ID = request_data;
                        client.Stream = tcp_client.GetStream();

                        lock(_Clients) {
                            _Clients.Add(client);
                            Console.WriteLine("{0}: Client " + request_data + " connected!", DateTime.Now.ToString("HH:mm:ss"));
                        }

                        break;
                    }
                    case "SendMessageToAll": {
                        var receivers = _Clients.FindAll(p => (p.ID != client.ID));

                        string response = client.ID + ": " + request_data;
                        Byte[] response_bytes = Encoding.ASCII.GetBytes(response);

                        foreach (var receiver in receivers) {
                            receiver.Stream.Write(response_bytes, 0, response_bytes.Length);
                            receiver.Stream.Flush();
                        }

                        break;
                    }
                }
            }
        }


        private TcpListener _Server;
        private List<ConnectedClient> _Clients;
    }

}