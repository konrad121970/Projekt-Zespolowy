using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Server.DataProcessing {

    internal class RequestDispatcher {
        public RequestDispatcher(BaseRequest request) {
            _Result = new RequestResult();
            _Request = request;
        }

        public void Dispatch<T>(Func<T, ClientInfo, RequestResult> function, ClientInfo client) where T : BaseRequest {
            if (_Request.GetType() == typeof(T)) {
                _Result = function(_Request as T, client);
            }
        }

        public RequestResult GetRequestResult() {
            return _Result;
        }

        private RequestResult _Result;
        private readonly BaseRequest _Request;
    }

}