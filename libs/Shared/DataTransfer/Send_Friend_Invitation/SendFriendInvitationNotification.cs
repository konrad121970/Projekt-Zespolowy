using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class SendFriendInvitationNotification : BaseNotification {
        public string SenderID { get; set; }
    }

}