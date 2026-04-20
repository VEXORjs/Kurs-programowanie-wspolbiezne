using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using App.Data;
using App.Logic;

namespace App.Presentation.ViewModel
{
    public class BallViewModel : INotifyPropertyChanged
    {
        private readonly IBallService _service;
        private readonly DispatcherTimer _timer;
        private readonly List<BallItemViewModel> _ballItems;

        private bool _isRunning;

        public ObservableCollection<BallItemViewModel> Balls { get; }

        public ICommand StartCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand ResetCommand { get; }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning == value)
                {
                    return;
                }

                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public BallViewModel(IBallService service)
        {
            _service = service;

            _ballItems = new List<BallItemViewModel>();
            Balls = new ObservableCollection<BallItemViewModel>();

            LoadBalls();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _timer.Tick += (_, __) => Update(0.016);

            StartCommand = new RelayCommand(Start);
            PauseCommand = new RelayCommand(Pause);
            ResetCommand = new RelayCommand(Reset);

            Start();
        }

        private void LoadBalls()
        {
            _ballItems.Clear();
            Balls.Clear();

            foreach (var ball in _service.GetBalls())
            {
                var item = new BallItemViewModel(ball);
                _ballItems.Add(item);
                Balls.Add(item);
            }
        }

        private void Update(double dt)
        {
            _service.UpdatePositions(_ballItems.Select(x => x.Model), dt);

            foreach (var item in _ballItems)
            {
                item.Refresh();
            }
        }

        private void Start()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            _timer.Start();
        }

        private void Pause()
        {
            if (!IsRunning)
            {
                return;
            }

            _timer.Stop();
            IsRunning = false;
        }

        private void Reset()
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

        public sealed class BallItemViewModel : INotifyPropertyChanged
        {
            public Ball Model { get; }

            public BallItemViewModel(Ball model)
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