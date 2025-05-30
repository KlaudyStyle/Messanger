using System;
using System.Windows;

namespace MyMessagerClient
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var token = await _authService.LoginAsync(
                    UsernameBox.Text,
                    PasswordBox.Password
                );

                AuthToken.Token = token;
                AuthToken.Username = UsernameBox.Text;

                OpenMainWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}");
            }
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var token = await _authService.RegisterAsync(
                    UsernameBox.Text,
                    PasswordBox.Password
                );

                AuthToken.Token = token;
                AuthToken.Username = UsernameBox.Text;

                OpenMainWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        private void OpenMainWindow()
        {
            this.Hide();

            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            this.Show();
        }
    }
}