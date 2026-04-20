using System.Windows;
using App.Data;
using App.Logic;
using App.Presentation.View;

namespace App.Presentation
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IBallRepository repo = new BallRepository();
            IBallService service = new BallService(repo);

            var window = new MainWindow(service);
            window.Show();
        }
    }
}