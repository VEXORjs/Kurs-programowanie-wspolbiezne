using System;

namespace App.Data
{
    public class Ball : IBall
    {
        private double _x;
        private double _y;
        private double _vx;
        private double _vy;

        public double X => _x;
        public double Y => _y;
        public double VX => _vx;
        public double VY => _vy;

        public double Radius { get; set; }
        public double Mass { get; set; } = 1.0;

        public void Move(double dt)
        {
            _x += _vx * dt;
            _y += _vy * dt;
        }

        public void SetPosition(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public void ApplyVelocity(double vx, double vy)
        {
            _vx = vx;
            _vy = vy;
        }

        public void BounceX()
        {
            _vx = -_vx;
        }

        public void BounceY()
        {
            _vy = -_vy;
        }

        public BallState GetState()
        {
            return new BallState(_x, _y, _vx, _vy);
        }

        public void ResolveCollision(
            double nx,
            double ny,
            double impulse,
            double otherMass)
        {
            _vx -= impulse * otherMass * nx;
            _vy -= impulse * otherMass * ny;
        }
    }
}