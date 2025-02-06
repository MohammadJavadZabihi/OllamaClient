using Ollama.Data.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace OllamaClient.Windows.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Message> Messages { get; set; }

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }

        public ICommand SendMessageCommand { get; }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<Message>();
            SendMessageCommand = new RelayCommand(SendMessage);
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(NewMessage))
            {
                Messages.Add(new Message { Content = NewMessage, IsUserChat = true });
                NewMessage = "";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
