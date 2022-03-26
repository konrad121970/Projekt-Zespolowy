using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class SendFriendInvitationNotification : INotification {
        public string SenderID { get; set; }
    }

}