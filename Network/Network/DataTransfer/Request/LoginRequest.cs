using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Network.Database;
using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

using Microsoft.AspNetCore.Identity;

namespace Network.DataTransfer.Request {

    [Serializable]
    public class LoginRequest : BaseRequest {
        internal override RequestResult Process(TcpClient client) {
            bool user_exists_in_db = false;

            using (var db = new DrocsidDbContext()) {
                var account_list = db.Accounts.ToList();
                var account = account_list.Find(p => (p.Username == Login));

                if(account != null) {
                    var passwordHasher = new PasswordHasher<string>();
                    user_exists_in_db = passwordHasher.VerifyHashedPassword(null, account.Password, Password) == PasswordVerificationResult.Success;
                }
            }

            if (user_exists_in_db) {
                var connected_client = new ConnectedClient() {
                    // User data 
                    UserID = Login,

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