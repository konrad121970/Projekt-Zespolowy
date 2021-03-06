using System;

using System.IO;
using System.Threading;

using Network.Client;

using Network.Shared.DataTransfer.Interface;
using Network.Shared.DataTransfer.Test.Client;

namespace ClientTest {

    class Program {
        static void Main(string[] args) {
            // Settings
            var response_count = 16384;
            var notification_count = 16384;

            Client.Instance.ResponseReceived += OnResponseReceived;
            Client.Instance.NotificationReceived += OnNotificationReceived;

            // Connect to server
            Client.Instance.Connect("127.0.0.1", 65535);
            Thread.Sleep(250);

            // Delete previous results
            var filepath = args[0];
            File.Delete(filepath);

            // Start testing
            Client.Instance.SendRequest(new ClientTestRequest() {
                ResponseCount = response_count,
                NotificationCount = notification_count
            });

            // Print progress
            while (AllResponseReceived == false || AllNotificationReceived == false) {
                Console.WriteLine("Responses: {0} / {1}", CurrentResponse, response_count);
                Console.WriteLine("Notifications: {0} / {1}", CurrentNotification, notification_count);

                Console.Write("\n");
                Thread.Sleep(500);
            }

            // Save results
            File.WriteAllText(filepath, ReceivedResponses + "\n" + ReceivedNotifications);
        }


        static void OnResponseReceived(object sender, IResponse response) {
            var server_response = (ClientTestResponse)response;

            CurrentResponse += 1;
            ReceivedResponses += "[" + DateTime.Now + "] = " + server_response.ID + "\n";

            if (server_response.LastResponse) {
                AllResponseReceived = true;
            }
        }

        static void OnNotificationReceived(object sender, INotification notification) {
            var server_notification = (ClientTestNotification)notification;

            CurrentNotification += 1;
            ReceivedNotifications += "[" + DateTime.Now + "] = " + server_notification.ID + "\n";

            if (server_notification.LastNotification) {
                AllNotificationReceived = true;
            }
        }


        static int CurrentResponse = 0;
        static int CurrentNotification = 0;

        static bool AllResponseReceived = false;
        static bool AllNotificationReceived = false;

        static string ReceivedResponses = "Responses: \n";
        static string ReceivedNotifications = "Notifications: \n";
    }

}