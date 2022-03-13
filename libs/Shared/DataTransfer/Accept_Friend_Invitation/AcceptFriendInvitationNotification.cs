﻿using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Notification {

    [Serializable]
    public class AcceptFriendInvitationNotification : BaseNotification {
        public string NewFriendID { get; set; }
    }

}