using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Notification {

    [Serializable]
    public class SendMessageNotification : BaseNotification {
        // Notification data
        public string SenderID { get; set; }
        public string MessageContent { get; set; }

        // Notification type
        internal new static string GetStaticType() { return typeof(SendMessageNotification).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}