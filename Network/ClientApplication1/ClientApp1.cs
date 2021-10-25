using System;
using Network;

namespace ClientApplication1 {
    class ClientApp1 {
        static void Main(string[] args) {
            var client = new Client();
            client.Connect("127.0.0.1", 65535);

            client.Login("Kasztan#0001");
            client.StartChat();
        }
    }
}
