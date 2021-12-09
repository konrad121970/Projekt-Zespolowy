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
            Client.Instance.NotificationReceived += OnNotificationReceived;
            Messages = new ObservableCollection<Message>();

            SendMessageCommand = new RelayCommand(obj => {
                var message = new Message();
                message.Content = Message;

                Client.Instance.SendRequest(new SendMessageRequest() { 
                    SenderID = Client.Data.UserID,
                    ReceiverID = "pudzian028",
                    MessageContent = Message
                });

                Messages.Add(message);
                Message = "";
            });
        }

        // Notification event handling
        private void OnNotificationReceived(object sender, BaseNotification notification) {
            var dispatcher = new NotificationDispatcher(notification);
            dispatcher.Dispatch<SendMessageNotification>(OnSendMessageNotification);
        }
        private void OnSendMessageNotification(SendMessageNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                Messages.Add(new Message() {
                    Content = notification.MessageContent
                });
            });
        }

        // Commands
        public RelayCommand SendMessageCommand { get; set; }

        // Properties
        public ObservableCollection<Message> Messages { get; set; }

        public string Message {
            get { 
                return _Message; 
            }
            set {
                _Message = value;
                OnPropertyChanged();
            }
        }

        private string _Message;
    }

}