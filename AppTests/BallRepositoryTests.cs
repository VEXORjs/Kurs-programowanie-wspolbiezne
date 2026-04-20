using Xunit;
using App.Data;
using System.Linq;

namespace AppTests
{
    public class BallRepositoryTests
    {
        [Fact]
        public void ShouldReturnInitialBalls()
        {
            IBallRepository repo = new BallRepository();

            var balls = repo.GetInitialBalls();

            Assert.NotNull(balls);
            Assert.Equal(4, balls.Count);

            Assert.All(balls, b =>
            {
                Assert.True(b.Radius > 0);
            });
        }
    }
}