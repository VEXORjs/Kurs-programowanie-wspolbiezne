using Xunit;
using App.Data;

namespace AppTests
{
    public class BallRepositoryTests
    {
        [Fact]
        public void ShouldReturnInitialBalls()
        {
            IBallRepository repo =
                new BallRepository();

            var balls =
                repo.GetInitialBalls(
                    4,
                    500,
                    500);

            Assert.NotNull(balls);

            Assert.Equal(4, balls.Count);

            Assert.All(balls, b =>
            {
                Assert.True(b.Radius > 0);
            });
        }
    }
}