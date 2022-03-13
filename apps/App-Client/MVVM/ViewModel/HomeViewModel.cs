using System.Collections.ObjectModel;

using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network.Client;
using Network.Client.DataProcessing;

using Network.Shared.DataTransfer.Base;
using Network.Shared.DataTransfer.Request;
using Network.Shared.DataTransfer.Notification;

namespace ClientApp.MVVM.ViewModel {

    class HomeViewModel : ObservableObject {
        public HomeViewModel() {
            AddToFriendList = new ObservableCollection<User>();
            Client.Instance.NotificationReceived += OnNotificationReceived;

            AcceptFriendCommand = new RelayCommand(obj => {
                var user = obj as User;

                Client.Instance.SendRequest(new AcceptFriendInvitationRequest() { 
                    NewFriendID = user.Id
                });

                AddToFriendList.Remove(user);
                FriendList.Add(user);
            });

            DeclineFriendCommand = new RelayCommand(obj => {
                var user = obj as User;
                AddToFriendList.Remove(user);
            });

            SendInvitationCommand = new RelayCommand(obj => {
                Client.Instance.SendRequest(new SendFriendInvitationRequest() {
                    ReceiverID = SearchBox
                });
            });
        }

        // Notification event handling
        private void OnNotificationReceived(object sender, BaseNotification notification) {
            var dispatcher = new NotificationDispatcher(notification);
            dispatcher.Dispatch<SendFriendInvitationNotification>(OnSendFriendInvitationNotification);
            dispatcher.Dispatch<AcceptFriendInvitationNotification>(OnAcceptFriendInvitationNotification);
        }

        private void OnSendFriendInvitationNotification(SendFriendInvitationNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                AddToFriendList.Add(new User {
                    Id = notification.SenderID
                });
            });
        }

        private void OnAcceptFriendInvitationNotification(AcceptFriendInvitationNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                FriendList.Add(new User {
                    Id = notification.NewFriendID
                });
            });
        }

        // Commands
        public RelayCommand AcceptFriendCommand { get; set; }
        public RelayCommand DeclineFriendCommand { get; set; }
        public RelayCommand SendInvitationCommand { get; set; }

        // User data
        public ObservableCollection<User> AddToFriendList { get; set; }
        public ObservableCollection<User> FriendList { get; set; }

        // Control data
        public string SearchBox {
            get {
                return _SearchBox;
            }
            set {
                _SearchBox = value;
                OnPropertyChanged();
            }
        }
        private string _SearchBox;
    }

}