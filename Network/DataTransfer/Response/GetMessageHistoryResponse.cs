using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.DataTransfer.Response {

    [Serializable]
    public class GetMessageHistoryResponse : BaseResponse {
        internal GetMessageHistoryResponse(ResponseResult result)
            : base(result) {
        }

        // Response data
        public List<string> Messages { get; set; }

        // Response type
        internal new static string GetStaticType() { return typeof(GetMessageHistoryResponse).Name; }
        internal override string GetResponseType() { return GetStaticType(); }
    }

}