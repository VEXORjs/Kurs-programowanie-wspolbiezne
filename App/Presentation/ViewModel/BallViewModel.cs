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
        private readonly SynchronizationContext _uiContext;

        private bool _isRunning;

        private readonly List<Brush> _colorPalette = new List<Brush>
        {
            Brushes.Red,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Orange,
            Brushes.Purple,
            Brushes.Cyan,
            Brushes.Magenta,
            Brushes.Yellow
        };

        private readonly Random _random = new Random();

        private CancellationTokenSource _cts;

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
                if (_boardWidth > 0 && _boardHeight > 0)
                {
                    LoadBalls();
                    Start();
                }
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
                if (_boardWidth > 0 && _boardHeight > 0)
                {
                    LoadBalls();
                    Start();
                }
            }
        }

        public BallViewModel(IBallService service)
        {
            _service = service;

            _uiContext = SynchronizationContext.Current;

            _ballItems = new List<BallItemViewModel>();
            Balls = new ObservableCollection<BallItemViewModel>();

            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            ResetCommand = new RelayCommand(Reset);

            _timer = new System.Timers.Timer(1000); // ~1 sek
            _timer.Elapsed += OnTimerElapsed;

            LoadBalls();
        }
        
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsRunning)
                return;

            _uiContext?.Post(_ =>
            {
                foreach (var item in _ballItems)
                {
                    int randomIndex = _random.Next(_colorPalette.Count);
                    item.BallColor = _colorPalette[randomIndex];
                    item.Refresh();
                }
            }, null);
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
            if (IsRunning) return;

            IsRunning = true;
            _timer.Start();

            _cts = new CancellationTokenSource();

            // Przechwytujemy bieżący kontekst wątku UI (wywołany na wątku głównym)
            var uiContext = SynchronizationContext.Current;
            var ballModels = _ballItems.Select(item => item.Model);

            Task.Run(() => _service.StartSimulationAsync(
                BoardWidth,
                BoardHeight,
                ballModels,
                () => {
                    // Bezpiecznie przesyłamy instrukcję do wątku UI
                    uiContext?.Post(_ =>
                    {
                        foreach (var item in _ballItems)
                        {
                            item.Refresh();
                        }
                    }, null);
                },
                _cts.Token
            ));
        }

        private void Pause()
        {
            _timer.Stop();
            IsRunning = false;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
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

            private Brush _ballColor = Brushes.Red;
            public Brush BallColor
            {
                get { return _ballColor; }
                set
                {
                    if (_ballColor == value) return;
                    _ballColor = value;
                    OnPropertyChanged();
                }
            }

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
