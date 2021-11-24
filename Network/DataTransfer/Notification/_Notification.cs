using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Notification {

    [Serializable]
    public abstract class BaseNotification {
        internal static string GetStaticType() { throw new NotImplementedException(); }
        internal virtual string GetResponseType() { return GetStaticType(); }
    }

    public class NotificationDispatcher {
        public NotificationDispatcher(BaseNotification notification) {
            _Notification = notification;
        }

        public void Dispatch<T>(Action<T> function) where T : BaseNotification {
            var method = typeof(T).GetMethod("GetStaticType", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var static_type = method.Invoke(null, Array.Empty<object>()) as string;

            if (_Notification.GetResponseType() == static_type) {
                function(_Notification as T);
            }
        }

        private readonly BaseNotification _Notification;
    }

}