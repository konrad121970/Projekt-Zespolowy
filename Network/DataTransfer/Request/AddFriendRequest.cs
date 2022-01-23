using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Network.Database;

using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace Network.DataTransfer.Request {

    [Serializable]
    public class AddFriendRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            var friends = new List<string>();
            bool pending_invitation = false;

            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == UserID)).First();
                var conversations = user_account.Conversations.Where(p => (p.Name == "..."));

                foreach (var acc in db.Accounts) {
                    if ((acc != user_account) && (acc.Conversations.Intersect(conversations).Any())) {
                        friends.Add(acc.Username);
                    }
                }

                if (!friends.Contains(FriendID)) {
                    var friend_account = db.Accounts.Where(p => (p.Username == FriendID)).First();

                    if(friend_account == null) {
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

            if(pending_invitation) {
                var notification_receivers = new List<TcpClient>();
                var receiver = Server.Data.ConnectedClients.Find(p => (p.UserID == FriendID));

                if (receiver != null) {
                    notification_receivers.Add(receiver.Client);
                }

                var notification_data = new AddFriendNotification() {
                    SenderID = UserID
                };

                var request_result = new RequestResult() {
                    NotificationReceivers = notification_receivers,
                    NotificationData = notification_data
                };

                return request_result;
            }

            return new RequestResult();
        }

        // Request data
        public string UserID { get; set; }
        public string FriendID { get; set; }
    }

}