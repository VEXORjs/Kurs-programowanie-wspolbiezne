using Xunit;
using System.Collections.Generic;
using App.Data;
using App.Logic;

namespace AppTests
{
    public class BallServiceTests
    {
        private class FakeBallRepository : IBallRepository
        {
            public IReadOnlyList<IBall> GetInitialBalls()
            {
                return new List<IBall>
                {
                    new Ball { X = 0, Y = 0, VX = 0, VY = 0, Radius = 10 }
                };
            }
        }

        [Fact]
        public void UpdatePositions_ShouldMoveBall()
        {
            IBallRepository repo = new FakeBallRepository();
            IBallService service = new BallService(repo);

            var balls = new List<IBall>
            {
                new Ball { X = 0, Y = 0, VX = 2, VY = 3, Radius = 10 }
            };

            service.UpdatePositions(balls, 1.0, 100, 100);

            Assert.Equal(2, balls[0].X);
            Assert.Equal(3, balls[0].Y);
        }

        [Fact]
        public void Ball_ShouldStopAtRightBoundary()
        {
            IBallRepository repo = new FakeBallRepository();
            IBallService service = new BallService(repo);

            var balls = new List<IBall>
            {
                new Ball { X = 90, Y = 10, VX = 50, VY = 0, Radius = 10 }
            };

            service.UpdatePositions(balls, 1.0, 100, 100);

            Assert.Equal(80, balls[0].X); 
            Assert.Equal(0, balls[0].VX); 
        }

        [Fact]
        public void Ball_ShouldStopAtBottomBoundary()
        {
            IBallRepository repo = new FakeBallRepository();
            IBallService service = new BallService(repo);

            var balls = new List<IBall>
            {
                new Ball { X = 10, Y = 90, VX = 0, VY = 50, Radius = 10 }
            };

            service.UpdatePositions(balls, 1.0, 100, 100);

            Assert.Equal(80, balls[0].Y);
            Assert.Equal(0, balls[0].VY);
        }
    }
}