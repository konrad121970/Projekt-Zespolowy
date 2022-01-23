using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Network.Database;
using Network.DataTransfer.Notification;

namespace Network.DataTransfer.Request {

    [Serializable]
    public class AcceptFriendRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == UserID)).First();
                var friend_account = db.Accounts.Where(p => (p.Username == FriendID)).First();

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
            var receiver = Server.Data.ConnectedClients.Find(p => (p.UserID == FriendID));

            if (receiver != null) {
                notification_receivers.Add(receiver.Client);
            }

            var notification_data = new AcceptFriendNotification() {
                UserID = UserID
            };

            var request_result = new RequestResult() {
                NotificationReceivers = notification_receivers,
                NotificationData = notification_data
            };

            return request_result;
        }

        // Request data
        public string UserID { get; set; }
        public string FriendID { get; set; }
    }

}