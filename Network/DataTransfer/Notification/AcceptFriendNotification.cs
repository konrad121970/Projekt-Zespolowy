using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Notification {

    [Serializable]
    public class AcceptFriendNotification : BaseNotification {
        // Notification data
        public string UserID { get; set; }

        // Notification type
        internal new static string GetStaticType() { return typeof(AcceptFriendNotification).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}