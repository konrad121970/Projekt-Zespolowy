using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Response {

    [Serializable]
    public class SendMessageResponse : BaseResponse {
        internal SendMessageResponse(ResponseResult result)
            : base(result) {
        }

        // Response data
        // { ... }

        // Response type
        internal new static string GetStaticType() { return typeof(SendMessageResponse).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}