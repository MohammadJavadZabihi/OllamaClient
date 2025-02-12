using System.Windows;
using Ollama.Core.Services;
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

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
