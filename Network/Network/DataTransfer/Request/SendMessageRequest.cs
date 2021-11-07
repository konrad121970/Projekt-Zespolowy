using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace Network.DataTransfer.Request {

    [Serializable]
    public class SendMessageRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            var receiver = Server.Data.ConnectedClients.Find(p => (p.UserID == ReceiverID));

            if(receiver == null) {
                return new RequestResult();
            }

            var notification_receivers = new List<TcpClient>();
            notification_receivers.Add(receiver.Client);
            
            var notification_data = new SendMessageNotification() {
                SenderID = this.SenderID,
                MessageContent = this.MessageContent
            };

            var request_result = new RequestResult() {
                NotificationReceivers = notification_receivers,
                NotificationData = notification_data
            };

            return request_result;
        }

        // Request data
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }

        public string MessageContent { get; set; }
    }

}