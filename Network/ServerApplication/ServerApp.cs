using Network;

namespace ServerApplication {

    class ServerApp {
        static void Main(string[] args) {
            Server.Instance.Start("0.0.0.0", 65535);
        }
    }

}