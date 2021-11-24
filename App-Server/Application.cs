using Network;

namespace ServerApp {

    class Application {
        static void Main(string[] args) {
            Server.Instance.Start("0.0.0.0", 65535);
        }
    }

}