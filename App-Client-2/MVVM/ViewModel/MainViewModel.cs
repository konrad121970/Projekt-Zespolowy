using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network;
using Network.DataTransfer.Request;
using Network.DataTransfer.Notification;

using ClientApp.Core;
using ClientApp.MVVM.Model;

namespace ClientApp.MVVM.ViewModel {

    class MainViewModel : ObservableObject {
        public MainViewModel() {
            CurrentUser = new User();
            CurrentUser.Id = Client.Data.UserID;

            FriendList = new ObservableCollection<User>();

            foreach (var friend_id in Client.Data.FriendList) {
                FriendList.Add(new User {
                    Id = friend_id
                });
            }

            ChatViewCommand = new RelayCommand(obj => {
                var user = (obj as User);

                ChatVM = new ChatViewModel(user);
                CurrentView = ChatVM;
            });

            HomeViewCommand = new RelayCommand(obj => {
                HomeVM = new HomeViewModel() {
                    FriendList = FriendList
                };

                CurrentView = HomeVM;
            });
        }

        //Commands

        public RelayCommand ChatViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }

        // Properties

        public ChatViewModel ChatVM { get; set; }
        public HomeViewModel HomeVM { get; set; }


        public User CurrentUser { get; set; }
        public ObservableCollection<User> FriendList { get; set; }
 

        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private object _currentView;
    }

}