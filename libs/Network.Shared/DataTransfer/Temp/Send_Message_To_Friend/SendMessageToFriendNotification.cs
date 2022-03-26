using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class SendMessageToFriendNotification : INotification {
        public string SenderID { get; set; }
        public string Content { get; set; }
    }

}