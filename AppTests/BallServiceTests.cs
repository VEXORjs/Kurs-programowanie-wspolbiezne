using Xunit;
using System.Collections.Generic;
using App.Data;
using App.Logic;

namespace AppTests
{    public class BallServiceTests
    {
        [Fact]
        public void UpdatePositions_ShouldMoveBall()
        {
            var repo = new FakeBallRepository();
            var service = new BallService(repo);

            var balls = new List<IBall>
            {
                new Ball { X = 0, Y = 0, VX = 2, VY = 3, Radius = 10 }
            };

            service.UpdatePositions(balls, 1.0, 100, 100);

            Assert.Equal(2, balls[0].X);
            Assert.Equal(3, balls[0].Y);
        }
    }
}