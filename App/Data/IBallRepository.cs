public interface IBallRepository
{
    IReadOnlyList<IBall> GetInitialBalls(int count, double width, double height);
}