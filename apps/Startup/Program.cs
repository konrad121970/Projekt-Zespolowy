namespace Startup {

    using System.Diagnostics;
    using System.Threading;

    class Program {
        static void Main(string[] args) {
            Process.Start("ServerApp.exe");

            for(int i = 0; i < 2; i++) {
                Process.Start("ClientApp.exe");
            }

            while(true) {
                Thread.Sleep(5000);
            }
        }
    }

}