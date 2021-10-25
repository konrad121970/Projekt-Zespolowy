using System;
using Network;

namespace ServerApplication {
    class ServerApp {
        static void Main(string[] args) {
            var server = new Server("0.0.0.0", 65535);
            server.Start();
        }
    }
}