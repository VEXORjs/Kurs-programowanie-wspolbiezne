using System.Windows;
using App.Data;
using App.Logic;
using App.Presentation.View;

namespace App.Presentation
{
    public partial class App : Application
    {
        private FileLogger _logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _logger = new FileLogger();

            IBallRepository repo =
                new BallRepository();

            IBallService service =
                new BallService(repo, _logger);

            var window = new MainWindow(service);

            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _logger?.Dispose();

            base.OnExit(e);
        }
    }
}