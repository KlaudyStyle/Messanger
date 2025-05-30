using System.Windows;

namespace MyMessagerClient
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                new MainWindow().Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}