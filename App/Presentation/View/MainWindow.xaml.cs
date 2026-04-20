using System.Windows;
using App.Logic;
using App.Presentation.ViewModel;

namespace App.Presentation.View
{
    public partial class MainWindow : Window
    {
        public MainWindow(IBallService service)
        {
            InitializeComponent();
            DataContext = new BallViewModel(service);
        }
    }
}