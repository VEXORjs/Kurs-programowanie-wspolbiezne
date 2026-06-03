using App.Data;
using App.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace App.Presentation.ViewModel
{
    public class BallViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IBallService _service;
        private readonly List<BallItemViewModel> _ballItems;
        private readonly System.Timers.Timer _timer;

        private bool _isRunning;

        public ObservableCollection<BallItemViewModel> Balls { get; }

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResetCommand { get; }

        public List<int> BallCounts { get; } = Enumerable.Range(1, 16).ToList();

        private int _selectedBallCount = 4;
        public int SelectedBallCount
        {
            get => _selectedBallCount;
            set
            {
                if (_selectedBallCount == value) return;
                _selectedBallCount = value;
                OnPropertyChanged();
                Reset();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning == value) return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        private double _boardWidth;
        public double BoardWidth
        {
            get => _boardWidth;
            set
            {
                if (_boardWidth == value) return;
                _boardWidth = value;
                OnPropertyChanged();
            }
        }

        private double _boardHeight;
        public double BoardHeight
        {
            get => _boardHeight;
            set
            {
                if (_boardHeight == value) return;
                _boardHeight = value;
                OnPropertyChanged();
            }
        }

        public BallViewModel(IBallService service)
        {
            _service = service;

            _ballItems = new List<BallItemViewModel>();
            Balls = new ObservableCollection<BallItemViewModel>();

            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            ResetCommand = new RelayCommand(Reset);

            _timer = new System.Timers.Timer(16); // ~60 FPS
            _timer.Elapsed += OnTimerElapsed;

            LoadBalls();

            Start();
        }
        
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsRunning)
                return;

            double dt = 0.016; // Assuming 60 FPS, so ~16ms per frame

            _service.UpdatePositions(
                _ballItems.Select(x => x.Model),
                dt,
                BoardWidth,
                BoardHeight);

            System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var item in _ballItems)
                {
                    item.Refresh();
                }
            });
        }

        public void Dispose()
        {
            if (_timer != null) {
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
            }
        }

        private void LoadBalls()
        {
            if (BoardWidth <= 0 || BoardHeight <= 0)
                return;

            _ballItems.Clear();
            Balls.Clear();

            foreach (var ball in _service.GetBalls(SelectedBallCount, BoardWidth, BoardHeight))
            {
                var item = new BallItemViewModel(ball);
                _ballItems.Add(item);
                Balls.Add(item);
            }
        }

        private void Start()
        {
            _timer.Start();
            IsRunning = true;
        }

        private void Pause()
        {
            _timer.Stop();
            IsRunning = false;
        }

        public void Reset()
        {
            Pause();
            LoadBalls();
            Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ============================
        // BALL ITEM VM
        // ============================

        public sealed class BallItemViewModel : INotifyPropertyChanged
        {
            public IBall Model { get; }

            public BallItemViewModel(IBall model)
            {
                Model = model;
            }

            public double X => Model.X;
            public double Y => Model.Y;
            public double Radius => Model.Radius;
            public double Diameter => Model.Radius * 2.0;

            public void Refresh()
            {
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(Radius));
                OnPropertyChanged(nameof(Diameter));
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // ============================
        // RELAY COMMAND
        // ============================

        private sealed class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute();
            }

            public void Execute(object parameter)
            {
                _execute();
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }
        }
    }
}
