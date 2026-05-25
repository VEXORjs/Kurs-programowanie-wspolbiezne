using System.Collections.Generic;
using App.Data;
using App.Logic;
using Xunit;

namespace AppTests
{
    public class BallServiceConsistencyTests
    {
        private class FakeRepo : IBallRepository
        {
            public IReadOnlyList<IBall> GetInitialBalls(int count, double width, double height)
            {
                return new List<IBall>();
            }
        }

        private class FakeLogger : ILogger
        {
            public void Log(string message) { }
        }

        [Fact]
        public void Simulation_Should_Keep_State_Valid_After_Update()
        {
            var repo = new FakeRepo();
            var service = new BallService(repo, new FakeLogger());

            var ball = new Ball
            {
                Radius = 10,
                Mass = 1
            };

            ball.SetPosition(50, 50);
            ball.ApplyVelocity(10, 0);

            var list = new List<IBall> { ball };

            service.UpdatePositions(list, 1.0, 100, 100);

            var state = ball.GetState();

            Assert.True(double.IsFinite(state.X));
            Assert.True(double.IsFinite(state.Y));
        }

        [Fact]
        public void Simulation_Should_Be_Deterministic_For_Same_Input()
        {
            var repo = new FakeRepo();
            var service = new BallService(repo, new FakeLogger());

            var a = new Ball { Radius = 10, Mass = 1 };
            var b = new Ball { Radius = 10, Mass = 1 };

            a.SetPosition(10, 10);
            a.ApplyVelocity(1, 0);

            b.SetPosition(40, 10);
            b.ApplyVelocity(-1, 0);

            var list1 = new List<IBall> { a, b };

            var a2 = new Ball { Radius = 10, Mass = 1 };
            var b2 = new Ball { Radius = 10, Mass = 1 };

            a2.SetPosition(10, 10);
            a2.ApplyVelocity(1, 0);

            b2.SetPosition(40, 10);
            b2.ApplyVelocity(-1, 0);

            var list2 = new List<IBall> { a2, b2 };

            service.UpdatePositions(list1, 1.0, 100, 100);
            service.UpdatePositions(list2, 1.0, 100, 100);

            Assert.Equal(list1[0].X, list2[0].X, precision: 5);
            Assert.Equal(list1[1].X, list2[1].X, precision: 5);
        }
    }
}