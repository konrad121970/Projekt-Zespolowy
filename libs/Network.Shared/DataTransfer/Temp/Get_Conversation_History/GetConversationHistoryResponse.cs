using System;
using System.Collections.Generic;
using Network.Shared.DataTransfer.Interface;

namespace Network.Shared.DataTransfer.Response {

    [Serializable]
    public class MessageInfo {
        public string SenderID { get; set; }
        public string Content { get; set; }
    }

    [Serializable]
    public class GetConversationHistoryResponse : IResponse {
        public List<MessageInfo> Messages { get; set; }
    }

}