using System.Windows;

namespace OllamaClient.Windows.View
{
    /// <summary>
    /// Interaction logic for SetChatTitleWindow.xaml
    /// </summary>
    public partial class SetChatTitleWindow : Window
    {
        public string ChatName { get; set; }

        public SetChatTitleWindow()
        {
            InitializeComponent();
        }

        private void NameSet_Click(object sender, RoutedEventArgs e)
        {
            ChatName = txtChatName.Text;
            DialogResult = true;

            this.Close();
        }
    }
}
