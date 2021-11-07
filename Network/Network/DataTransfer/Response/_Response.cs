using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Response {

    public enum ResponseResult {
        NONE, SUCCESS, FAILURE
    }

    [Serializable]
    public abstract class BaseResponse {
        internal BaseResponse(ResponseResult result) {
            Result = result;
        }

        // Response result
        public ResponseResult Result { get; set; }

        // Response type
        internal static string GetStaticType() { throw new NotImplementedException(); }
        internal virtual string GetResponseType() { return GetStaticType(); }
    }

    public class ResponseDispatcher {
        public ResponseDispatcher(BaseResponse response) {
            _Response = response;
        }

        public void Dispatch<T>(Action<T> function) where T : BaseResponse {
            var method = typeof(T).GetMethod("GetStaticType", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            var static_type = method.Invoke(null, Array.Empty<object>()) as string;

            if (_Response.GetResponseType() == static_type) {
                function(_Response as T);
            }
        }

        private readonly BaseResponse _Response;
    }

}