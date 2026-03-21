using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using App.Presentation.ViewModel;

namespace App.Presentation.View
{
    public partial class MainWindow : Window
    {
        private readonly BallViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new BallViewModel();
            DrawBalls();
        }

        private void DrawBalls()
        {
            foreach (var ball in _viewModel.Balls)
            {
                var ellipse = new Ellipse
                {
                    Width = ball.Radius * 2,
                    Height = ball.Radius * 2,
                    Fill = Brushes.Blue
                };

                Canvas.SetLeft(ellipse, ball.X * 20+100);
                Canvas.SetTop(ellipse, ball.Y * 20+100);

                BallCanvas.Children.Add(ellipse);
            }
        }
    }
}