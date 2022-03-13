using System;
using System.Collections.Generic;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Response {

    [Serializable]
    public class LogInToAccountResponse : BaseResponse {
        public string UserID { get; set; }
        public List<string> FriendIDs { get; set; }
    }

}