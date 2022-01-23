using Network.Database;
using Network.DataTransfer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Request {

    [Serializable]
    public class GetMessageHistoryRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            var all_messages = new List<string>();

            using (var db = new DrocsidDbContext()) {
                var user_account = db.Accounts.Where(p => (p.Username == UserID)).First();
                var user_conversations = user_account.Conversations;

                var friend_account = db.Accounts.Where(p => (p.Username == FriendID)).First();
                var friend_conversations = friend_account.Conversations;

                var conversation = user_conversations.Intersect(friend_conversations).First();
                var conversation_id = conversation.ID;

                var messages = db.Messages.Where(p => p.Conversation_ID == conversation_id).OrderBy(p => p.SendDate).ToList();
                
                foreach(var message in messages) {
                    all_messages.Add(message.Content);
                }
            }

            var response_data = new GetMessageHistoryResponse(ResponseResult.SUCCESS) {
                Messages = all_messages
            };

            var request_result = new RequestResult() {
                ResponseReceiver = client,
                ResponseData = response_data
            };

            return request_result;
        }

        // Request data
        public string UserID { get; set; }
        public string FriendID { get; set; }
    }

}