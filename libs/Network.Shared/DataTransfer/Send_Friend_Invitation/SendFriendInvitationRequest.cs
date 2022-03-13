using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class SendFriendInvitationRequest : BaseRequest {
        public string ReceiverID { get; set; }
    }

}