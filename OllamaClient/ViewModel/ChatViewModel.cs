using Ollama.Core.Services;
using Ollama.Core.ViewModels;
using Ollama.Data.Entites;
using Ollama.Data.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OllamaClient.Windows.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Message> Messages { get; set; }
        public ObservableCollection<Chat> Chats { get; set; }

        private readonly ChatDataBaseService _chatDataBaseService;
        private readonly ApiService _apiService;
        private readonly OllamaService _ollamaService;


        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get => _selectedChat;
            set
            {
                _selectedChat = value;
                OnPropertyChanged();
            }
        }

        private string _selectedModel;
        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged();
            }
        }

        private string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand { get; }
        public ICommand LoadChatMessagesCommand { get; }
        public ICommand SelectChatCommand { get; }

        public ChatViewModel(ChatDataBaseService chatDataBaseService, ApiService apiService, OllamaService ollamaService)
        {
            _chatDataBaseService = chatDataBaseService;
            Chats = new ObservableCollection<Chat>();
            _apiService = apiService;
            Messages = new ObservableCollection<Message>();
            _ollamaService = ollamaService;

            SendMessageCommand = new RelayCommand(async () => await SendMessage(), () => !string.IsNullOrEmpty(NewMessage));
            LoadChatMessagesCommand = new RelayCommand(async () => await LoadChatMessages());
            SelectChatCommand = new RelayCommand<Chat>(async (chat) => await SelectChat(chat));
        }

        private async Task LoadChatMessages()
        {
            if (SelectedChat == null) return;

            Messages.Clear();
            var messages = await _chatDataBaseService.GetMessagesForChat(SelectedChat.ChatId);

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        public async Task SelectChat(Chat chat)
        {
            if (chat == null) return;

            SelectedChat = chat;
            await LoadChatMessages();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(NewMessage)) return;

            IsLoading = true;

            if (SelectedChat == null)
            {
                var newChat = new Chat
                {
                    ChatTitle = $"{NewMessage} چت جدید با عنوان" + DateTime.Now.ToString("HH:mm")
                };

                await _chatDataBaseService.AddChatTitle(new AddChatViewModel { ChatTitle = newChat.ChatTitle, IUserChat = true, Content = NewMessage });
                Chats.Add(newChat);
                SelectedChat = newChat;
            }

            var userMessage = new Message
            {
                Content = NewMessage,
                IsUserChat = true
            };
            Messages.Add(userMessage);

            await _chatDataBaseService.AddChatTitle(new AddChatViewModel
            {
                ChatTitle = SelectedChat.ChatTitle,
                IUserChat = true,
                Content = NewMessage
            });

            var requestData = new
            {
                model = SelectedModel,
                messages = new[]
                {
                    new { role = "user", content = NewMessage }
                },
                stream = false
            };


            string response = await _apiService.SendPostRequest(requestData);

            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
            string botContent = jsonObject?.message?.content ?? "";

            var botMessage = new Message
            {
                Content = botContent,
                IsUserChat = false
            };
            Messages.Add(botMessage);
            await _chatDataBaseService.AddChatTitle(new AddChatViewModel
            {
                ChatTitle = SelectedChat.ChatTitle,
                Content = botContent,
                IUserChat = false
            });

            NewMessage = string.Empty;
            IsLoading = false;
            await GetChatTitle();
        }

        public async Task GetChatTitle()
        {
            var chatList = await _chatDataBaseService.GetMessageTitle();

            Chats.Clear();
            foreach (var chat in chatList)
            {
                Chats.Add(chat);
            }
        }

        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<T, bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
