using System.Threading;

using System.Windows;
using System.Windows.Controls;

using Network.Client;
using Network.Client.DataProcessing;

using Network.Shared.DataTransfer.Interface;

using Network.Shared.DataTransfer.Request;
using Network.Shared.DataTransfer.Response;

using ClientApp.Core;
using ClientApp.MVVM.View;
using System.Collections.Generic;

namespace ClientApp.MVVM.ViewModel {
    
    class LoginViewModel : ObservableObject {
        public LoginViewModel() {
            LoginCommand = new RelayCommand(obj => {
                var passwordBox = obj as PasswordBox;

                Client.Instance.Connect("127.0.0.1", 65535);
                Client.Instance.ResponseReceived += OnResponseReceived;

                Client.Instance.SendRequest(new LogInToAccountRequest() {
                    Login = Login,
                    Password = passwordBox.Password
                });

                while (!LoggedIn) {
                    Thread.Sleep(8);
                }

                var chatWindow = new MainWindow();
                (chatWindow.DataContext as MainViewModel).SetFriendList(FriendList);
                chatWindow.Show();

                Application.Current.Windows[0].Close();
            });
        }

        // Response event handling
        static void OnResponseReceived(object sender, IResponse response) {
            var dispatcher = new ResponseDispatcher(response);
            dispatcher.Dispatch<LogInToAccountResponse>(OnLogInToAccountResponse);
        }

        static void OnLogInToAccountResponse(LogInToAccountResponse response) {
            MessageBox.Show("Signed in as " + response.UserID);

            Client.Data.UserID = response.UserID;
            FriendList = response.FriendIDs;

            LoggedIn = true;
        }

        // Commands
        public RelayCommand LoginCommand { get; set; }
        public static bool LoggedIn = false;

        public static List<string> FriendList = new List<string>();

        // Control data
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