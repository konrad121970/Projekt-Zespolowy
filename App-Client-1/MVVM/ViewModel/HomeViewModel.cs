using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network;
using Network.DataTransfer.Notification;
using Network.DataTransfer.Request;

namespace ClientApp.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
            AddToFriendList = new ObservableCollection<User>();
            Client.Instance.NotificationReceived += OnNotificationReceived;

            AcceptFriendCommand = new RelayCommand(obj=>
            {
                var user = obj as User;

                Client.Instance.SendRequest(new AcceptFriendRequest() { 
                    UserID = Client.Data.UserID,
                    FriendID = user.Id
                });

                AddToFriendList.Remove(user);
                FriendList.Add(user);
            });

            DeclineFriendCommand = new RelayCommand(obj =>
            {
                var user = (obj as User);
                AddToFriendList.Remove(user);
            });

            SendInvitationCommand = new RelayCommand(obj => 
            {
                Client.Instance.SendRequest(new AddFriendRequest() {
                    UserID = Client.Data.UserID,
                    FriendID = SearchBox
                });
            });
        }

        // Notification event handling
        private void OnNotificationReceived(object sender, BaseNotification notification) {
            var dispatcher = new NotificationDispatcher(notification);
            dispatcher.Dispatch<AddFriendNotification>(OnAddFriendNotification);
            dispatcher.Dispatch<AcceptFriendNotification>(OnAcceptFriendNotification);
        }

        private void OnAddFriendNotification(AddFriendNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                AddToFriendList.Add(new User {
                    Id = notification.SenderID
                });
            });
        }

        private void OnAcceptFriendNotification(AcceptFriendNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                FriendList.Add(new User {
                    Id = notification.UserID
                });
            });
        }

        // Properties
        public ObservableCollection<User> AddToFriendList { get; set; }
        public ObservableCollection<User> FriendList { get; set; }

        public string SearchBox
        {
            get
            {
                return _searchBox;
            }
            set
            {
                _searchBox = value;
                OnPropertyChanged();
            }
        }
        private string _searchBox;

        //Commands
        public RelayCommand AcceptFriendCommand { get; set; }
        public RelayCommand DeclineFriendCommand { get; set; }
        public RelayCommand SendInvitationCommand { get; set; }
    }
}