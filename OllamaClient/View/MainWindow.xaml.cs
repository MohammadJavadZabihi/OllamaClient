using System.Windows;
using Ollama.Core.Services;
using OllamaClient.Windows.View;
using OllamaClient.Windows.ViewModel;

namespace OllamaClient
{
    public partial class MainWindow : Window
    {
        private ChatDataBaseService _chatDbService = new ChatDataBaseService();
        private ApiService _apiService = new ApiService();
        private OllamaService _ollamaService = new OllamaService();

        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new ChatViewModel(_chatDbService, _apiService, _ollamaService);
            DataContext = viewModel;

            var models = _ollamaService.GetOllamaModelList();
            foreach (var model in models)
            {
                if (model != "NAME")
                {
                    cmbModel.Items.Add(model);
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ((ChatViewModel)DataContext).GetChatTitle();
        }

        private async void btnAddChat_Click(object sender, RoutedEventArgs e)
        {
            SetChatTitleWindow setChatTitleWindow = new SetChatTitleWindow();
            setChatTitleWindow.ShowDialog();

            if(string.IsNullOrEmpty(setChatTitleWindow.ChatName))
            {
                setChatTitleWindow.ChatName = "چت جدید با عنوان" + DateTime.Now.ToString("HH:mm:ss");
            }

            await ((ChatViewModel)DataContext).StartNewChat(setChatTitleWindow.ChatName);
        }
    }
}
