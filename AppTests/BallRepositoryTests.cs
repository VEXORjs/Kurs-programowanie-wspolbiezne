using Xunit;
using App.Data;

namespace AppTests
{
    public class BallRepositoryTests
    {
        [Fact]
        public void ShouldReturnInitialBalls()
        {
            var repo = new BallRepository();

            var balls = repo.GetInitialBalls();

            Assert.NotNull(balls);
            Assert.Equal(3, balls.Count);
        }
    }
}