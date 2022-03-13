using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class LogInToAccountRequest : BaseRequest {
        public string Login { get; set; }
        public string Password { get; set; }
    }

}