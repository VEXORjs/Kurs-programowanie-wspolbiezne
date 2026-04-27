public interface IBall
{
    double X { get; set; }
    double Y { get; set; }
    double VX { get; set; }
    double VY { get; set; }
    double Radius { get; }
    double Mass { get; }

    object Lock { get; }
}