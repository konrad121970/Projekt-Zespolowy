using System;
using System.Diagnostics;

namespace Startup {

    enum STARTUP {
        MAIN_APPLICATION = 0,
        NETWORK_CLIENT_TEST, NETWORK_SERVER_TEST, BOTH_NETWORK_TESTS
    }

    class Program {
        static void Main(string[] args) {
            // Settings
            var app_type = STARTUP.MAIN_APPLICATION;
            var client_count = 2;

            // Init processes
            var server_process = Process.Start("ServerApp.exe");
            var client_processes = new Process[client_count * 2];

            // Start processes
            for(int i = 0; i < client_count; i++) {
                switch (app_type) {
                    case STARTUP.MAIN_APPLICATION: {
                        client_processes[i] = RunClientServerApplication();
                        break;
                    }
                    case STARTUP.NETWORK_CLIENT_TEST: {
                        client_processes[i] = RunNetworkClientTest(i + 1);
                        break;
                    }
                    case STARTUP.NETWORK_SERVER_TEST: {
                        client_processes[i] = RunNetworkServerTest(i + 1);
                        break;
                    }
                    case STARTUP.BOTH_NETWORK_TESTS: {
                        client_processes[(i * 2) + 0] = RunNetworkClientTest(i + 1);
                        client_processes[(i * 2) + 1] = RunNetworkServerTest(i + 1);
                        break;
                    }
                }
            }

            // Wait for server process
            server_process.WaitForExit();

            // Kill all client processes
            foreach (var process in client_processes) {
                if (process != null && process.HasExited == false) {
                    process.Kill();
                }
            }
        }

        // Applications
        static Process RunClientServerApplication() {
            return Process.Start("ClientApp.exe");
        }

        static Process RunNetworkClientTest(int id) {
            return Process.Start("ClientTest.exe", "ClientTestLog" + id + ".txt");
        }

        static Process RunNetworkServerTest(int id) {
            return Process.Start("ServerTest.exe", "ServerTestLog" + id + ".txt");
        }
    }

}