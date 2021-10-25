using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWPF.Core;
using TestWPF.MVVM.Model;

namespace TestWPF.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }

        public RelayCommand SendCommand { get; set; }

        public string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();

            SendCommand = new RelayCommand(obj => 
            {
                Messages.Add(new MessageModel
                {
                    Message = Message
                });
                Message = "";
            });

           /*Messages.Add(new MessageModel
            {
                Message = "test1"
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Message = "test" + i
                });
            }*/
        }
    }
}
