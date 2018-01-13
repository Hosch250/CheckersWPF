using CheckersWPF.Pages;
using System.Windows;

namespace CheckersWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var board = new GamePage();

            MainWindow = new Window
            {
                Content = board
            };

            MainWindow.Show();
        }
    }
}
