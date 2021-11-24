using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Czat.Core;

namespace Czat.MVVM.ViewModel
{
    class LoginViewModel : ObservableObject
    {
        public RelayCommand SendCommand { get; set; }


        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                OnPropertyChanged();
            }
        }
        private string _Login;



        public LoginViewModel()
        {
            SendCommand = new RelayCommand(obj =>
            {
                var pb = (obj as PasswordBox);
                MessageBox.Show(_Login);      //login
                MessageBox.Show(pb.Password); //hasło

            });
        }
    }
}