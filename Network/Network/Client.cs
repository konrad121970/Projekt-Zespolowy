using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network {

    public class Client {
        public Client() {
            _Client = new TcpClient();
        }

        public void Connect(string server_ip, int server_port) {
            _Client.Connect(server_ip, server_port);

            Task.Run(() => {
                var stream = _Client.GetStream();

                while (true) {
                    Byte[] bytes = new Byte[_Client.Available];
                    stream.Read(bytes, 0, bytes.Length);

                    if (bytes.Length == 0) {
                        continue;
                    }

                    Console.WriteLine(Encoding.UTF8.GetString(bytes));
                }
            });
        }


        public void Login(string user_id) {
            Byte[] requestBytes = Encoding.ASCII.GetBytes("Login: " + user_id);
            var stream = _Client.GetStream();

            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Flush();
        }

        public void StartChat() {
            var stream = _Client.GetStream();

            while (true) {
                string message = Console.ReadLine();
                Byte[] bytes = Encoding.ASCII.GetBytes("SendMessageToAll: " + message);

                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
        }


        private TcpClient _Client;
    }

}