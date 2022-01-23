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
    public class SendMessageRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            var notification_receivers = new List<TcpClient>();
            var receiver = Server.Data.ConnectedClients.Find(p => (p.UserID == ReceiverID));

            if(receiver != null) {
                notification_receivers.Add(receiver.Client);
            }
            
            var notification_data = new SendMessageNotification() {
                SenderID = this.SenderID,
                MessageContent = this.MessageContent
            };

            var request_result = new RequestResult() {
                NotificationReceivers = notification_receivers,
                NotificationData = notification_data
            };

            using (var db = new DrocsidDbContext()) {
                var account = db.Accounts.Where(p => (p.Username == SenderID)).First();
                var account_id = account.ID;

                var sender_account = db.Accounts.Where(p => (p.Username == SenderID)).First();
                var sender_conversations = sender_account.Conversations;

                var receiver_account = db.Accounts.Where(p => (p.Username == ReceiverID)).First();
                var receiver_conversations = receiver_account.Conversations;

                var conversation = sender_conversations.Intersect(receiver_conversations).First();
                var conversation_id = conversation.ID;

                db.Messages.Add(new Message() { 
                    Sender_ID = account_id,
                    Conversation_ID = conversation_id,

                    Content = MessageContent,
                    SendDate = DateTime.Now
                });

                db.SaveChanges();
            }

            return request_result;
        }

        // Request data
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }

        public string MessageContent { get; set; }
    }

}