using System.Collections.Generic;
using App.Data;

namespace AppTests
{
    internal class FakeBallRepository : IBallRepository
    {
        public IReadOnlyList<IBall> GetInitialBalls(
            int count,
            double width,
            double height)
        {
            var balls = new List<IBall>();

            for (int i = 0; i < count; i++)
            {
                var ball = new Ball
                {
                    Radius = 10,
                    Mass = 1
                };

                ball.SetPosition(
                    i * 30,
                    i * 30);

                balls.Add(ball);
            }

            return balls;
        }
    }
}