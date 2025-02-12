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

        public ChatViewModel(ChatDataBaseService chatDataBaseService, ApiService apiService, OllamaService ollamaService)
        {
            _chatDataBaseService = chatDataBaseService;
            Chats = new ObservableCollection<Chat>();
            _apiService = apiService;
            Messages = new ObservableCollection<Message>();
            _ollamaService = ollamaService;

            SendMessageCommand = new RelayCommand(async () => await SendMessage(), () => !string.IsNullOrEmpty(NewMessage));
        }


        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(NewMessage))
                return;

            IsLoading = true;

            var userMessage = new Message
            {
                Content = NewMessage,
                IsUserChat = true
            };
            Messages.Add(userMessage);
            await _chatDataBaseService.AddChatTitle(new AddChatViewModel
            {
                ChatTitle = "Chat " + (Messages.Count),
                Content = NewMessage,
                IUserChat = true
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
                ChatTitle = "Chat " + (Messages.Count),
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
            foreach(var chat in chatList)
            {
                Chats.Add(chat);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
