using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class SendMessageToFriendRequest : IRequest {
        public string ReceiverID { get; set; }
        public string Content { get; set; }
    }

}