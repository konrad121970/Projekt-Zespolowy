using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Notification {

    [Serializable]
    public class LoginNotification : BaseNotification {
        // Notification data
        // { ... }

        // Notification type
        internal new static string GetStaticType() { return typeof(LoginNotification).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}