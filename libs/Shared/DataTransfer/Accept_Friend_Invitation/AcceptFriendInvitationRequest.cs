using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class AcceptFriendInvitationRequest : BaseRequest {
        public string NewFriendID { get; set; }
    }

}