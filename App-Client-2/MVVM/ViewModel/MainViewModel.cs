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
            
            FriendList = new ObservableCollection<User>();

            FriendList.Add(new User 
            {
                Id ="Mariusz#1862"
            });

            FriendList.Add(new User
            {
                Id = "Darek#1862"
            });

            FriendList.Add(new User
            {
                Id = "Szpaq#1862"
            });

            ChatViewCommand = new RelayCommand(obj=> 
            {
                var user = (obj as User);
                ChatVM = new ChatViewModel();
                ChatVM.CurrentUser = user;
                CurrentView = ChatVM;
            });

            HomeViewCommand = new RelayCommand(obj => 
            {
                HomeVM = new HomeViewModel();
                HomeVM.FriendList = FriendList;
                CurrentView = HomeVM;
            });
        }

        //Commands

        public RelayCommand ChatViewCommand { get; set; }
        public RelayCommand HomeViewCommand { get; set; }

        // Properties

        public ChatViewModel ChatVM { get; set; }
        public HomeViewModel HomeVM { get; set; }

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