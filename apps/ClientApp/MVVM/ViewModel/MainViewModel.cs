using System.Collections.ObjectModel;

using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network.Client;

namespace ClientApp.MVVM.ViewModel {

    class MainViewModel : ObservableObject {
        public MainViewModel() {
            FriendList = new ObservableCollection<User>();

            CurrentUser = new User();
            CurrentUser.Id = Client.Data.UserID;

            foreach (var friend_id in Client.Data.FriendList) {
                FriendList.Add(new User {
                    Id = friend_id
                });
            }

            HomeViewCommand = new RelayCommand(obj => {
                HomeVM = new HomeViewModel() {
                    FriendList = FriendList
                };

                CurrentView = HomeVM;
            });

            ChatViewCommand = new RelayCommand(obj => {
                var user = obj as User;

                ChatVM = new ChatViewModel(user);
                CurrentView = ChatVM;
            });
        }

        // Commands
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand ChatViewCommand { get; set; }

        // VM's
        public HomeViewModel HomeVM { get; set; }
        public ChatViewModel ChatVM { get; set; }

        // User data
        public User CurrentUser { get; set; }
        public ObservableCollection<User> FriendList { get; set; }
 
        // Control data
        public object CurrentView {
            get {
                return _CurrentView;
            }
            set {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }
        private object _CurrentView;
    }

}