using System;
using System.Collections.Generic;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Response {

    [Serializable]
    public class LogInToAccountResponse : IResponse {
        public string UserID { get; set; }
        public List<string> FriendIDs { get; set; }
    }

}