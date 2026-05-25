using System.Collections.Generic;
using System.Threading.Tasks;
using App.Data;
using App.Logic;
using Xunit;

namespace AppTests
{
    public class BallServiceStressTests
    {
        private class FakeRepo : IBallRepository
        {
            public IReadOnlyList<IBall> GetInitialBalls(int count, double width, double height)
                => new List<IBall>();
        }

        private class FakeLogger : ILogger
        {
            public void Log(string message) { }
        }

        [Fact]
        public void Simulation_Should_Not_Crash_Under_Concurrency()
        {
            var repo = new FakeRepo();
            var service = new BallService(repo, new FakeLogger());

            var balls = new List<IBall>();

            for (int i = 0; i < 20; i++)
            {
                var b = new Ball
                {
                    Radius = 10,
                    Mass = 1
                };

                b.SetPosition(i * 5, i * 5);
                b.ApplyVelocity(1, 1);

                balls.Add(b);
            }

            Parallel.For(0, 1000, _ =>
            {
                service.UpdatePositions(balls, 0.016, 500, 500);
            });

            foreach (var b in balls)
            {
                var s = b.GetState();
                Assert.True(double.IsFinite(s.X));
                Assert.True(double.IsFinite(s.Y));
            }
        }
    }
}