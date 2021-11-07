﻿using System;
using Network;

using Network.DataTransfer.Request;
using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace ClientApplication1 {

    class ClientApp1 {
        static void Main(string[] args) {
            // Connect to server
            Client.Instance.Connect("127.0.0.1", 65535);
            
            // Set event handling
            Client.Instance.ResponseReceived += OnResponseReceived;
            Client.Instance.NotificationReceived += OnNotificationReceived;

            // Login to account
            Client.Instance.SendRequest(new LoginRequest() {
                Login = "Andrzej",
                Password = "Password1"
            });

            // Send requests
            while (true) {
                var message = Console.ReadLine();

                Client.Instance.SendRequest(new SendMessageRequest() {
                    SenderID = Client.Data.UserID,
                    ReceiverID = "Kasztan#0002",

                    MessageContent = message
                });

                Client.Instance.SendRequest(new SendMessageRequest() {
                    SenderID = Client.Data.UserID,
                    ReceiverID = "Mariusz#0003",

                    MessageContent = message
                });
            }

            // Client app main thread
            // ...
        }


        // Response event handling
        static void OnResponseReceived(object sender, BaseResponse response) {
            var dispatcher = new ResponseDispatcher(response);

            dispatcher.Dispatch<LoginResponse>(OnLoginResponse);
            dispatcher.Dispatch<SendMessageResponse>(OnSendMessageResponse);
        }

        static void OnLoginResponse(LoginResponse response) {
            switch (response.Result) {
                case ResponseResult.SUCCESS: {
                    Console.WriteLine("Signed in as " + response.UserID);
                    Client.Data.UserID = response.UserID;
                    break;
                }
                case ResponseResult.FAILURE: {
                    Console.WriteLine("Incorrect login or password!");
                    break;
                }
            }
        }

        static void OnSendMessageResponse(SendMessageResponse response) {

        }


        // Notification event handling
        static void OnNotificationReceived(object sender, BaseNotification notification) {
            var dispatcher = new NotificationDispatcher(notification);

            dispatcher.Dispatch<LoginNotification>(OnLoginNotification);
            dispatcher.Dispatch<SendMessageNotification>(OnSendMessageNotification);
        }

        static void OnLoginNotification(LoginNotification notification) {

        }

        static void OnSendMessageNotification(SendMessageNotification notification) {
            Console.WriteLine("{0}: {1}", notification.SenderID, notification.MessageContent);
        }
    }

}