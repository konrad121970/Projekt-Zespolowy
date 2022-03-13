using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class SendMessageToFriendRequest : BaseRequest {
        public string ReceiverID { get; set; }
        public string Content { get; set; }
    }

}