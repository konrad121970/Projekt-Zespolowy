using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Response {

    [Serializable]
    public class LoginResponse : BaseResponse {
        internal LoginResponse(ResponseResult result) 
            : base(result) {
        }

        // Response data
        public string UserID { get; set; }

        // Response type
        internal new static string GetStaticType() { return typeof(LoginResponse).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}