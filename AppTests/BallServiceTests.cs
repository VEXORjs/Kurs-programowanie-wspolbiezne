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
            public IReadOnlyList<IBall> GetInitialBalls(
                int count,
                double width,
                double height)
            {
                return new List<IBall>();
            }
        }

        [Fact]
        public void UpdatePositions_ShouldMoveBall()
        {
            // arrange
            IBallRepository repo = new FakeBallRepository();
            ILogger logger = new FakeLogger();

            IBallService service =
                new BallService(repo, logger);

            var ball = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball.SetPosition(0, 0);
            ball.ApplyVelocity(2, 3);

            var balls = new List<IBall> { ball };

            // act
            service.UpdatePositions(
                balls,
                1.0,
                100,
                100);

            // assert
            Assert.Equal(2, ball.X);
            Assert.Equal(3, ball.Y);
        }

        [Fact]
        public void Ball_ShouldBounceAtRightBoundary()
        {
            // arrange
            IBallRepository repo = new FakeBallRepository();
            ILogger logger = new FakeLogger();

            IBallService service =
                new BallService(repo, logger);

            var ball = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball.SetPosition(90, 10);
            ball.ApplyVelocity(50, 0);

            var balls = new List<IBall> { ball };

            // act
            service.UpdatePositions(
                balls,
                1.0,
                100,
                100);

            // assert
            Assert.Equal(80, ball.X);
            Assert.True(ball.VX < 0);
        }

        [Fact]
        public void Ball_ShouldBounceAtBottomBoundary()
        {
            // arrange
            IBallRepository repo = new FakeBallRepository();
            ILogger logger = new FakeLogger();

            IBallService service =
                new BallService(repo, logger);

            var ball = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball.SetPosition(10, 90);
            ball.ApplyVelocity(0, 50);

            var balls = new List<IBall> { ball };

            // act
            service.UpdatePositions(
                balls,
                1.0,
                100,
                100);

            // assert
            Assert.Equal(80, ball.Y);
            Assert.True(ball.VY < 0);
        }

        [Fact]
        public void Balls_ShouldBounceAfterCollision()
        {
            // arrange
            IBallRepository repo = new FakeBallRepository();
            ILogger logger = new FakeLogger();

            IBallService service =
                new BallService(repo, logger);

            var ball1 = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            var ball2 = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball1.SetPosition(0, 0);
            ball1.ApplyVelocity(1, 0);

            ball2.SetPosition(15, 0);
            ball2.ApplyVelocity(-1, 0);

            var balls = new List<IBall>
            {
                ball1,
                ball2
            };

            // act
            service.UpdatePositions(
                balls,
                1.0,
                100,
                100);

            // assert
            Assert.True(ball1.VX < 1);
            Assert.True(ball2.VX > -1);
        }
    }
}
