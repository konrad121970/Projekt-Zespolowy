using System;
using Network.Shared.DataTransfer.Base;

namespace Network.Client.DataProcessing {

    public class ResponseDispatcher {
        public ResponseDispatcher(BaseResponse response) {
            _Response = response;
        }

        public void Dispatch<T>(Action<T> function) where T : BaseResponse {
            if (_Response.GetType() == typeof(T)) {
                function(_Response as T);
            }
        }

        private readonly BaseResponse _Response;
    }

}