using System;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Request {

    [Serializable]
    public class LogInToAccountRequest : IRequest {
        public string Login { get; set; }
        public string Password { get; set; }
    }

}