using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class AcceptFriendInvitationNotification : INotification {
        public string NewFriendID { get; set; }
    }

}