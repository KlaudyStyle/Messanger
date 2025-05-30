using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MessengerClient
{
    public partial class MainWindow : Window
    {
        private readonly MessageService _messageService = new MessageService();
        private bool _usernameSet = false;
        private DispatcherTimer _refreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUsername();
            SetupRefreshTimer();
        }

        private void SetupRefreshTimer()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5);
            _refreshTimer.Tick += async (s, e) => await LoadMessagesAsync();
            _refreshTimer.Start();
        }

        private async void InitializeUsername()
        {
            var savedUsername = UsernameManager.LoadUsername();
            if (!string.IsNullOrEmpty(savedUsername))
            {
                UsernameInput.Text = savedUsername;
                LockUsername();
                await LoadMessagesAsync();
            }
        }

        private void LockUsername()
        {
            UsernameInput.IsEnabled = false;
            UseUsernameButton.IsEnabled = false;
            _usernameSet = true;
        }

        private async void UseUsername_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(UsernameInput.Text))
            {
                UsernameManager.SaveUsername(UsernameInput.Text);
                LockUsername();
                await LoadMessagesAsync();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UsernameManager.DeleteUsername();
            UsernameInput.Text = "";
            UsernameInput.IsEnabled = true;
            UseUsernameButton.IsEnabled = true;
            _usernameSet = false;
            MessagesListView.ItemsSource = null;
        }

        private async Task LoadMessagesAsync()
        {
            if (!_usernameSet) return;

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
            if (!_usernameSet)
            {
                MessageBox.Show("Сначала установите имя пользователя");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MessageInputBox.Text))
            {
                try
                {
                    bool success = await _messageService.SendMessageAsync(
                        UsernameInput.Text,
                        MessageInputBox.Text
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
        }
    }
}