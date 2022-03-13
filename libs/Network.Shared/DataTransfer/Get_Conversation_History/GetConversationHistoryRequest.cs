using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class GetConversationHistoryRequest : BaseRequest {
        public string FriendID { get; set; }
        public string ChannelID { get; set; }
    }

}