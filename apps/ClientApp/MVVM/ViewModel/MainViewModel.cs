using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network.Client;

namespace ClientApp.MVVM.ViewModel {

    class MainViewModel : ObservableObject {
        public MainViewModel() {

            FriendList = new ObservableCollection<User>();
            //dodaje
            FriendListVisibility = Visibility.Visible;
            ServerListVisibility = Visibility.Hidden;
            //

            CurrentUser = new User();
            CurrentUser.Id = Client.Data.UserID;
            HomeVM = new HomeViewModel()
            {
                FriendList = FriendList
            };

            HomeViewCommand = new RelayCommand(obj => {
              
                CurrentView = HomeVM;
            });

            ChatViewCommand = new RelayCommand(obj => {
                var user = obj as User;
                
                ChatVM = new ChatViewModel(user);

                CurrentView = ChatVM;
            });
            //to dodałem
            FriendListViewCommand = new RelayCommand(obj => {
                MakeFriendListVisible();
            });

            ServerListViewCommand = new RelayCommand(obj => {
                MakeServerListVisible();
                ServerVM = new ServerViewModel();
                CurrentView = ServerVM;
            });

            //
        }

        public void SetFriendList(List<string> friends) {
            foreach (var friend_id in friends) {
                FriendList.Add(new User {
                    Id = friend_id
                });
            }
        }

        public void MakeFriendListVisible() 
        {
                ServerListVisibility = Visibility.Hidden;
                FriendListVisibility = Visibility.Visible;
        }

        public void MakeServerListVisible()
        {
            ServerListVisibility = Visibility.Visible;
            FriendListVisibility = Visibility.Hidden;
        }

        // Commands
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FriendListViewCommand { get; set; }
        public RelayCommand ServerListViewCommand { get; set; }
        public RelayCommand ChatViewCommand { get; set; }

        // VM's
        public HomeViewModel HomeVM { get; set; }
        public ChatViewModel ChatVM { get; set; }
        public ServerViewModel ServerVM { get; set; }

        // User data
        public User CurrentUser { get; set; }
        public ObservableCollection<User> FriendList { get; set; }

        // Control data
        public object CurrentView 
        {
            get {
                return _CurrentView;
            }
            set {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }
        private object _CurrentView;

        public Visibility FriendListVisibility
        {
            get
            {
                return _FriendListVisibility;
            }
            set
            {
                _FriendListVisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility _FriendListVisibility;

        public Visibility ServerListVisibility
        {
            get
            {
                return _ServerListVisibility;
            }
            set
            {
                _ServerListVisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility _ServerListVisibility;
    }

}