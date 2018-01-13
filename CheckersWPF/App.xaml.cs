using CheckersWPF.Facade;
using CheckersWPF.Pages;
using CheckersWPF.VMs;
using System.Windows;
using System.Windows.Navigation;

namespace CheckersWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var gamePage = new GamePage
            {
                DataContext = new GamePageViewModel()
            };

            var boardEditor = new BoardEditor
            {
                DataContext = new BoardEditorViewModel(Board.DefaultBoard(Variant.AmericanCheckers))
            };

            var rules = new Rules
            {
                DataContext = new RulesViewModel()
            };

            var board = new MainPage(gamePage, boardEditor, rules);

            MainWindow = new NavigationWindow
            {
                Content = board,
                ShowsNavigationUI = false
            };

            MainWindow.Show();
        }
    }
}
