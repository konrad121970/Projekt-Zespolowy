namespace Startup {

    using System.Diagnostics;
    using System.Threading;

    class Program {
        static void Main(string[] args) {
            var processes = new Process[CLIENT_APP_COUNT + 1];
            processes[0] = Process.Start("ServerApp.exe");

            for(int i = 1; i <= CLIENT_APP_COUNT; i++) {
                processes[i] = Process.Start("ClientApp.exe");
            }

            while(!processes[0].HasExited) {
                Thread.Sleep(1000);
            }

            foreach(var process in processes) {
                if(!process.HasExited) {
                    process.Kill();
                }
            }
        }

        static int CLIENT_APP_COUNT = 2;
    }

}