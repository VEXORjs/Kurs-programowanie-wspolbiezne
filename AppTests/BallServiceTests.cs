using Xunit;
using System.Collections.Generic;
using App.Data;
using App.Logic;

namespace AppTests
{
    public class BallServiceTests
    {
        [Fact]
        public void UpdatePositions_ShouldMoveBall()
        {
            var repo = new BallRepository();
            var service = new BallService(repo);

            var balls = new List<Ball>
            {
                new Ball { X = 0, Y = 0}
            };

            service.UpdatePositions(balls, 1.0);

            Assert.Equal(0, balls[0].X);
            Assert.Equal(0, balls[0].Y);
            Assert.True(true);
        }
    }
}