using ApiRequest.Net.CallApi;
using Newtonsoft.Json;
using Ollama.Core.Services;
using Ollama.Data.Entities;
using OllamaClient.Windows.ViewModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OllamaClient
{
    public partial class MainWindow : Window
    {
        private CallApi _callApi;
        private ApiService _apiService;
        private OllamaService _ollamaService;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ChatViewModel();
            _callApi = new CallApi();
            _apiService = new ApiService();
            _ollamaService = new OllamaService();
        }

        private async void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            var requestData = new
            {
                model = cmbModel.Text,
                messages = new[]
                {
                    new { role = "user", content = txtSendMessage.Text }
                },
                stream = false
            };

            var viewModel = DataContext as ChatViewModel;

            viewModel.IsLoading = true;

            string response = await _apiService.SendPostRequest(requestData);

            viewModel.IsLoading = false;

            var jsonObject = JsonConvert.DeserializeObject<dynamic>(response);
            string content = jsonObject?.message?.content ?? "";

            viewModel?.Messages.Add(new Message { Content = content, IsUserChat = false});

            txtSendMessage.Text = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var modles = _ollamaService.GetOllamaModelList();
            foreach (var model in modles)
            {
                if (model != "NAME")
                {
                    cmbModel.Items.Add(model);
                }
            }
        }
    }
}