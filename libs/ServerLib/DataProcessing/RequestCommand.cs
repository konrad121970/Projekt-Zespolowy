using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Sockets;

using Network.Server.Database;

using Network.Shared.DataTransfer.Base;
using Network.Shared.DataTransfer.Request;
using Network.Shared.DataTransfer.Response;
using Network.Shared.DataTransfer.Notification;

namespace Network.Server.DataProcessing {

    public class RequestResult {
        // Response
        public TcpClient ResponseReceiver { get; set; }
        public BaseResponse ResponseData { get; set; }

        // Notification
        public List<TcpClient> NotificationReceivers { get; set; }
        public BaseNotification NotificationData { get; set; }
    }

    internal static class RequestCommand {
        public static RequestResult Dispatch(BaseRequest request, ClientInfo client) {
            var dispatcher = new RequestDispatcher(request);

            dispatcher.Dispatch<LogInToAccountRequest>(LogInToAccount, client);
            dispatcher.Dispatch<GetConversationHistoryRequest>(GetConversationHistory, client);

            dispatcher.Dispatch<SendFriendInvitationRequest>(SendFriendInvitation, client);
            dispatcher.Dispatch<AcceptFriendInvitationRequest>(AcceptFriendInvitation, client);

            dispatcher.Dispatch<SendMessageToFriendRequest>(SendMessageToFriend, client);

            return dispatcher.GetRequestResult();
        }

        private static RequestResult LogInToAccount(LogInToAccountRequest request, ClientInfo client) {
            bool user_exists_in_db = false;

            using (var db = new DrocsidDbContext()) {
                var account_list = db.Accounts.ToList();
                var account = account_list.Find(p => (p.Username == request.Login));

                if (account != null) {
                    user_exists_in_db = (account.Password == request.Password) ? true : false;
                }
            }

            if (user_exists_in_db) {
                client.UserID = request.Login;

                Server.Data.Clients.Add(client);
                Console.WriteLine("{0} connected to the server!", client.UserID);

                var friends = new List<string>();
                using (var db = new DrocsidDbContext()) {
                    var account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                    var conversations = account.Conversations.Where(p => (p.Name == "..."));

                    foreach (var acc in db.Accounts) {
                        if ((acc != account) && (acc.Conversations.Intersect(conversations).Any())) {
                            friends.Add(acc.Username);
                        }
                    }
                }

                var response_data = new LogInToAccountResponse() {
                    UserID = client.UserID,
                    FriendIDs = friends
                };

                var request_result = new RequestResult() {
                    ResponseReceiver = client.TCP,
                    ResponseData = response_data
                };

                return request_result;
            }

            return new RequestResult();
        }
        private static RequestResult GetConversationHistory(GetConversationHistoryRequest request, ClientInfo client) {
            var all_messages = new List<MessageInfo>();

            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var user_conversations = user_account.Conversations;

                var friend_account = db.Accounts.Where(p => (p.Username == request.FriendID)).First();
                var friend_conversations = friend_account.Conversations;

                var conversation = user_conversations.Intersect(friend_conversations).First();
                var conversation_id = conversation.ID;

                var messages = db.Messages.Where(p => p.Conversation_ID == conversation_id).OrderBy(p => p.SendDate).ToList();

                foreach (var message in messages) {
                    all_messages.Add(new MessageInfo() {
                        SenderID = "XD",
                        Content = message.Content
                    });
                }
            }

            var response_data = new GetConversationHistoryResponse() {
                Messages = all_messages
            };

            var request_result = new RequestResult() {
                ResponseReceiver = client.TCP,
                ResponseData = response_data
            };

            return request_result;
        }

        private static RequestResult SendFriendInvitation(SendFriendInvitationRequest request, ClientInfo client) {
            var friends = new List<string>();
            bool pending_invitation = false;

            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var conversations = user_account.Conversations.Where(p => (p.Name == "..."));

                foreach (var acc in db.Accounts) {
                    if ((acc != user_account) && (acc.Conversations.Intersect(conversations).Any())) {
                        friends.Add(acc.Username);
                    }
                }

                if (!friends.Contains(request.ReceiverID)) {
                    var friend_account = db.Accounts.Where(p => (p.Username == request.ReceiverID)).First();

                    if (friend_account == null) {
                        return null;
                    }

                    db.PendingFriendInvitations.Add(new PendingFriendInvitation() {
                        Sender_ID = user_account.ID,
                        Receiver_ID = friend_account.ID
                    });
                    db.SaveChanges();

                    pending_invitation = true;
                }
            }

            if (pending_invitation) {
                var notification_receivers = new List<TcpClient>();
                var receiver = Server.Data.Clients.Find(p => (p.UserID == request.ReceiverID));

                if (receiver != null) {
                    notification_receivers.Add(receiver.TCP);
                }

                var notification_data = new SendFriendInvitationNotification() {
                    SenderID = client.UserID
                };

                var request_result = new RequestResult() {
                    NotificationReceivers = notification_receivers,
                    NotificationData = notification_data
                };

                return request_result;
            }

            return new RequestResult();
        }
        private static RequestResult AcceptFriendInvitation(AcceptFriendInvitationRequest request, ClientInfo client) {
            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var friend_account = db.Accounts.Where(p => (p.Username == request.NewFriendID)).First();

                var invitation = db.PendingFriendInvitations.Where(p => (p.Sender_ID == friend_account.ID) && (p.Receiver_ID == user_account.ID)).First();
                db.PendingFriendInvitations.Remove(invitation);

                var conversation = db.Conversations.Add(new Conversation() {
                    Name = "..."
                });

                user_account.Conversations.Add(conversation);
                friend_account.Conversations.Add(conversation);

                db.SaveChanges();
            }

            var notification_receivers = new List<TcpClient>();
            var receiver = Server.Data.Clients.Find(p => (p.UserID == request.NewFriendID));

            if (receiver != null) {
                notification_receivers.Add(receiver.TCP);
            }

            var notification_data = new AcceptFriendInvitationNotification() {
                NewFriendID = client.UserID
            };

            var request_result = new RequestResult() {
                NotificationReceivers = notification_receivers,
                NotificationData = notification_data
            };

            return request_result;
        }

        private static RequestResult SendMessageToFriend(SendMessageToFriendRequest request, ClientInfo client) {
            var notification_receivers = new List<TcpClient>();
            var receiver = Server.Data.Clients.Find(p => (p.UserID == request.ReceiverID));

            if (receiver != null) {
                notification_receivers.Add(receiver.TCP);
            }

            var notification_data = new SendMessageToFriendNotification() {
                SenderID = client.UserID,
                Content = request.Content
            };

            var request_result = new RequestResult() {
                NotificationReceivers = notification_receivers,
                NotificationData = notification_data
            };

            using (var db = new DrocsidDbContext()) {
                var account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var account_id = account.ID;

                var sender_account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var sender_conversations = sender_account.Conversations;

                var receiver_account = db.Accounts.Where(p => (p.Username == client.UserID)).First();
                var receiver_conversations = receiver_account.Conversations;

                var conversation = sender_conversations.Intersect(receiver_conversations).First();
                var conversation_id = conversation.ID;

                db.Messages.Add(new Message() {
                    Sender_ID = account_id,
                    Conversation_ID = conversation_id,

                    Content = request.Content,
                    SendDate = DateTime.Now
                });

                db.SaveChanges();
            }

            return request_result;
        }
    }

}