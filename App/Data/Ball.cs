namespace App.Data
{
    public class Ball : IBall
    {
        private readonly object _lock = new object();

        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }

        public double Radius { get; set; }
        public double Mass { get; set; } = 1.0;

        public object Lock => _lock;
    }
}