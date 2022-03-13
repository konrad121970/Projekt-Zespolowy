using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class SendMessageToFriendNotification : BaseNotification {
        public string SenderID { get; set; }
        public string Content { get; set; }
    }

}