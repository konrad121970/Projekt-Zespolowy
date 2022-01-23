using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network;
using Network.DataTransfer.Request;
using Network.DataTransfer.Response;
using Network.DataTransfer.Notification;

namespace ClientApp.MVVM.ViewModel {

    class ChatViewModel : ObservableObject
    {
        public ChatViewModel(User user)
        {
            Messages = new ObservableCollection<Message>();
            CurrentUser = user;

            Client.Instance.ResponseReceived += OnResponseReceived;
            Client.Instance.NotificationReceived += OnNotificationReceived;

            Client.Instance.SendRequest(new GetMessageHistoryRequest() { 
                UserID = Client.Data.UserID,
                FriendID = CurrentUser.Id
            });

            SendMessageCommand = new RelayCommand(obj => {
                var message = new Message();
                message.Content = Message;

                Client.Instance.SendRequest(new SendMessageRequest()
                {
                    SenderID = Client.Data.UserID,
                    ReceiverID = CurrentUser.Id,

                    MessageContent = Message
                });

                Messages.Add(message);
                Message = "";
            });
        }

        // Response event handling
        void OnResponseReceived(object sender, BaseResponse response) {
            var dispatcher = new ResponseDispatcher(response);
            dispatcher.Dispatch<GetMessageHistoryResponse>(OnGetMessageHistoryResponse);
        }

        void OnGetMessageHistoryResponse(GetMessageHistoryResponse response) {
            App.Current.Dispatcher.Invoke(delegate {
                foreach (var message in response.Messages) {
                    Messages.Add(new Message() {
                        Content = message
                    });
                }
            });
        }

        // Notification event handling
        private void OnNotificationReceived(object sender, BaseNotification notification)
        {
            var dispatcher = new NotificationDispatcher(notification);
            dispatcher.Dispatch<SendMessageNotification>(OnSendMessageNotification);
        }
        private void OnSendMessageNotification(SendMessageNotification notification)
        {
            App.Current.Dispatcher.Invoke(delegate {
                Messages.Add(new Message()
                {
                    Content = notification.MessageContent
                });
            });
        }

        // Commands
        public RelayCommand SendMessageCommand { get; set; }

        public User CurrentUser { get; set; }

        // Properties
        public ObservableCollection<Message> Messages { get; set; }
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyChanged();
            }
        }

        private string _Message;
    }

}