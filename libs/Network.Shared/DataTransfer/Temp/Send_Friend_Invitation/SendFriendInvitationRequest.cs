using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class SendFriendInvitationRequest : IRequest {
        public string ReceiverID { get; set; }
    }

}