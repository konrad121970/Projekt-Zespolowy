using System;
using System.Collections.Generic;
using Network.Shared.DataTransfer.Base;

namespace Network.Shared.DataTransfer.Response {

    [Serializable]
    public class MessageInfo {
        public string SenderID { get; set; }
        public string Content { get; set; }
    }

    [Serializable]
    public class GetConversationHistoryResponse : BaseResponse {
        public List<MessageInfo> Messages { get; set; }
    }

}