namespace ServerApp {

    using Network;
    using Network.Server;

    class Program {
        static void Main(string[] args) {
            Server.Instance.Start("0.0.0.0", 65535);
        }
    }

}