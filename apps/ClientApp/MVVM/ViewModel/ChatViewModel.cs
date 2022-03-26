using System.Collections.ObjectModel;

using ClientApp.Core;
using ClientApp.MVVM.Model;

using Network.Client;
using Network.Client.DataProcessing;

using Network.Shared.DataTransfer.Interface;

using Network.Shared.DataTransfer.Request;
using Network.Shared.DataTransfer.Response;
using Network.Shared.DataTransfer.Notification;

namespace ClientApp.MVVM.ViewModel {

    class ChatViewModel : ObservableObject {
        public ChatViewModel(User user) {
            CurrentUser = user;

            this.Message = "";
            Messages = new ObservableCollection<Message>();

            Client.Instance.ResponseReceived += OnResponseReceived;
            Client.Instance.NotificationReceived += OnNotificationReceived;

            Client.Instance.SendRequest(new GetConversationHistoryRequest() { 
                FriendID = CurrentUser.Id
            });

            SendMessageCommand = new RelayCommand(obj => {
                var message = new Message();
                message.Content = this.Message;

                Client.Instance.SendRequest(new SendMessageToFriendRequest() {
                    ReceiverID = CurrentUser.Id,
                    Content = message.Content
                });

                Messages.Add(message);
                this.Message = "";
            });
        }

        // Response event handling
        void OnResponseReceived(object sender, IResponse response) {
            var dispatcher = new ResponseDispatcher(response);
            dispatcher.Dispatch<GetConversationHistoryResponse>(OnGetConversationHistoryResponse);
        }

        void OnGetConversationHistoryResponse(GetConversationHistoryResponse response) {
            App.Current.Dispatcher.Invoke(delegate {
                foreach (var message in response.Messages) {
                    Messages.Add(new Message() {
                        Content = message.Content
                    });
                }
            });
        }

        // Notification event handling
        private void OnNotificationReceived(object sender, INotification notification) {
            var dispatcher = new NotificationDispatcher(notification);
            dispatcher.Dispatch<SendMessageToFriendNotification>(OnSendMessageToFriendNotification);
        }

        private void OnSendMessageToFriendNotification(SendMessageToFriendNotification notification) {
            App.Current.Dispatcher.Invoke(delegate {
                Messages.Add(new Message() {
                    Content = notification.Content
                });
            });
        }

        // Commands
        public RelayCommand SendMessageCommand { get; set; }

        // User data
        public User CurrentUser { get; set; }

        // Message to send
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

        // Message history
        public ObservableCollection<Message> Messages { get; set; }
    }

}