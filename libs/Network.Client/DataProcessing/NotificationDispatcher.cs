using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Client.DataProcessing {

    public class NotificationDispatcher {
        public NotificationDispatcher(BaseNotification notification) {
            _Notification = notification;
        }

        public void Dispatch<T>(Action<T> function) where T : BaseNotification {
            if (_Notification.GetType() == typeof(T)) {
                function(_Notification as T);
            }
        }

        private readonly BaseNotification _Notification;
    }

}