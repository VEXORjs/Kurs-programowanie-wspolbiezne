namespace App.Data
{
    public interface IBall
    {
        double X { get; }
        double Y { get; }

        double VX { get; }
        double VY { get; }

        double Radius { get; }
        double Mass { get; }

        void Move(double dt);

        void SetPosition(double x, double y);

        void ApplyVelocity(double vx, double vy);

        void BounceX();

        void BounceY();

        BallState GetState();

        void ResolveCollision(
            double nx,
            double ny,
            double impulse,
            double otherMass);
    }
    public readonly struct BallState
    {
        public BallState(
            double x,
            double y,
            double vx,
            double vy)
        {
            X = x;
            Y = y;
            VX = vx;
            VY = vy;
        }

        public double X { get; }
        public double Y { get; }
        public double VX { get; }
        public double VY { get; }
    }
}
