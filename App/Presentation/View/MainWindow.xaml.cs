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

        private void Board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is BallViewModel vm)
            {
                vm.BoardWidth = e.NewSize.Width;
                vm.BoardHeight = e.NewSize.Height;

                vm.Reset();
            }
        }
    }
}