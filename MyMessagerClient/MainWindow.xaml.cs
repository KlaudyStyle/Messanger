using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MyMessagerClient
{
    public partial class MainWindow : Window
    {
        private readonly MessageService _messageService = new MessageService();
        private DispatcherTimer _refreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            UsernameText.Text = AuthToken.Username;
            SetupRefreshTimer();
        }

        private void SetupRefreshTimer()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5);
            _refreshTimer.Tick += async (s, e) => await LoadMessagesAsync();
            _refreshTimer.Start();
            _ = LoadMessagesAsync();
        }

        private async Task LoadMessagesAsync()
        {
            try
            {
                var messages = await _messageService.GetMessagesAsync();
                MessagesListView.ItemsSource = messages;
            }
            catch (Exception ex)
            {
            }
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInputBox.Text)) return;

            try
            {
                bool success = await _messageService.SendMessageAsync(
                    MessageInputBox.Text,
                    AuthToken.Token
                );

                if (success)
                {
                    MessageInputBox.Text = "";
                    await LoadMessagesAsync();
                }
                else
                {
                    MessageBox.Show("Не удалось отправить сообщение");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки: {ex.Message}");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AuthToken.Token = null;
            AuthToken.Username = null;

            new LoginWindow().Show();
            Close();
        }
    }
}