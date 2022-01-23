using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Notification {

    [Serializable]
    public class AddFriendNotification : BaseNotification {
        // Notification data
        public string SenderID { get; set; }

        // Notification type
        internal new static string GetStaticType() { return typeof(AddFriendNotification).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}