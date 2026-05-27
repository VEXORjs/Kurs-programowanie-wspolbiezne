using System;

namespace App.Data
{
    public class Ball : IBall
    {
        private readonly object _lock = new();

        private double _x;
        private double _y;

        private double _vx;
        private double _vy;

        public double X
        {
            get
            {
            //zamek(programowanie wspolbiezne
                lock (_lock)
                {
                    return _x;
                }
            }
        }

        public double Y
        {
            get
            {
                lock (_lock)
                {
                    return _y;
                }
            }
        }

        public double VX
        {
            get
            {
                lock (_lock)
                {
                    return _vx;
                }
            }
        }

        public double VY
        {
            get
            {
                lock (_lock)
                {
                    return _vy;
                }
            }
        }

        public double Radius { get; set; }

        public double Mass { get; set; } = 1.0;

        public object Lock => _lock;

        public void Move(double dt)
        {
            lock (_lock)
            {
            //aktualiazacja stanu zależna od rzeczywistego upływu czasu
                _x += _vx * dt;
                _y += _vy * dt;
            }
        }

        public void SetPosition(double x, double y)
        {
            lock (_lock)
            {
                _x = x;
                _y = y;
            }
        }

        public void ApplyVelocity(double vx, double vy)
        {
            lock (_lock)
            {
                _vx = vx;
                _vy = vy;
            }
        }

        public void BounceX()
        {
            lock (_lock)
            {
                _vx = -_vx;
            }
        }

        public void BounceY()
        {
            lock (_lock)
            {
                _vy = -_vy;
            }
        }

        public BallState GetState()
        {
            lock (_lock)
            {
                return new BallState(
                    _x,
                    _y,
                    _vx,
                    _vy);
            }
        }

        public void ResolveCollision(
            double nx,
            double ny,
            double impulse,
            double otherMass)
        {
            lock (_lock)
            {
                _vx -= impulse * otherMass * nx;
                _vy -= impulse * otherMass * ny;
            }
        }
    }
}
