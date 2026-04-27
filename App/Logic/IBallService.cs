public interface IBallService
{
    IReadOnlyList<IBall> GetBalls(int count, double width, double height);

    void UpdatePositions(
        IEnumerable<IBall> balls,
        double dt,
        double width,
        double height);
}