using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Network;
using Network.DataTransfer.Request;
using Network.DataTransfer.Response;

using ClientApp.Core;
using ClientApp.MVVM.Model;

namespace ClientApp.MVVM.ViewModel {
    
    class LoginViewModel : ObservableObject {
        public LoginViewModel() {
            LoginCommand = new RelayCommand(obj => {
                var passwordBox = (obj as PasswordBox);

                Client.Instance.Connect("127.0.0.1", 65535);
                Client.Instance.ResponseReceived += OnResponseReceived;

                Client.Instance.SendRequest(new LoginRequest() { 
                    Login = this.Login,
                    Password = passwordBox.Password
                });
            });
        }

        // Response event handling
        static void OnResponseReceived(object sender, BaseResponse response) {
            var dispatcher = new ResponseDispatcher(response);
            dispatcher.Dispatch<LoginResponse>(OnLoginResponse);
        }

        static void OnLoginResponse(LoginResponse response) {
            switch (response.Result) {
                case ResponseResult.SUCCESS: {
                    MessageBox.Show("Signed in as " + response.UserID);
                    Client.Data.UserID = response.UserID;
                    break;
                }
                case ResponseResult.FAILURE: {
                    MessageBox.Show("Incorrect login or password!");
                    break;
                }
            }
        }

        // Commands
        public RelayCommand LoginCommand { get; set; }

        // Properties
        public string Login {
            get {
                return _Login;
            }
            set {
                _Login = value;
                OnPropertyChanged();
            }
        }
        private string _Login;
    }

}