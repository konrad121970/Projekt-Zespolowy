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
    public class LoginRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            var userLoginDetails = Server.Data.UserLoginData.Find(p => (p.Item1 == Login) && (p.Item2 == Password));

            if (userLoginDetails != null) {
                var connected_client = new ConnectedClient() {
                    // User data 
                    UserID = userLoginDetails.Item3,

                    // Connection data
                    Client = client,
                    Stream = client.GetStream()
                };

                Server.Data.ConnectedClients.Add(connected_client);
                Console.WriteLine("{0} connected to the server!", connected_client.UserID);

                var response_data = new LoginResponse(ResponseResult.SUCCESS) {
                    UserID = connected_client.UserID
                };

                var request_result = new RequestResult() {
                    ResponseReceiver = client,
                    ResponseData = response_data
                };

                return request_result;
            }
            else {
                var request_result = new RequestResult() {
                    ResponseReceiver = client,
                    ResponseData = new LoginResponse(ResponseResult.FAILURE)
                };

                return request_result;
            }
        }


        // Request data
        public string Login { get; set; }
        public string Password { get; set; }
    }

}