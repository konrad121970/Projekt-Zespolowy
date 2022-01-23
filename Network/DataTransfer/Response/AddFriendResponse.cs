using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Response {

    [Serializable]
    public class AddFriendResponse : BaseResponse {
        internal AddFriendResponse(ResponseResult result)
            : base(result) {
        }

        // Response data
        // { ... }

        // Response type
        internal new static string GetStaticType() { return typeof(AddFriendResponse).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}