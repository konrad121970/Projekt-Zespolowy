using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Core;
using ClientApp.MVVM.Model;

namespace ClientApp.MVVM.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
            AddToFriendList = new ObservableCollection<User>();
            AddToFriendList.Add(new User
            {
                Id = "Kurinox#1862"
            });

            AcceptFriendCommand = new RelayCommand(obj=>
            {
                var user = (obj as User);
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
                //fukcja dodaj(SearchBox);
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
