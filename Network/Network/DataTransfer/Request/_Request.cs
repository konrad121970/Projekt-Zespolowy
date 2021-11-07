using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace Network.DataTransfer.Request {

    [Serializable]
    public abstract class BaseRequest {
        internal abstract RequestResult Process(TcpClient client);
    }

    class RequestResult {
        // Response
        public TcpClient ResponseReceiver { get; set; }
        public BaseResponse ResponseData { get; set; }

        // Notification
        public List<TcpClient> NotificationReceivers { get; set; }
        public BaseNotification NotificationData { get; set; }
    }

}