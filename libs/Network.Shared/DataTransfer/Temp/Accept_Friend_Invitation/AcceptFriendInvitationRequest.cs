using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class AcceptFriendInvitationRequest : IRequest {
        public string NewFriendID { get; set; }
    }

}