using System;
using Network;

namespace ClientApplication2 {
    class ClientApp2 {
        static void Main(string[] args) {
            var client = new Client();
            client.Connect("127.0.0.1", 65535);

            client.Login("Andrzej#0002");
            client.StartChat();
        }
    }
}