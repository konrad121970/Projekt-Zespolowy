using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class GetConversationHistoryRequest : IRequest {
        public string FriendID { get; set; }
        public string ChannelID { get; set; }
    }

}