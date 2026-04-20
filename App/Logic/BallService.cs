using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public class BallService : IBallService
    {
        private readonly IBallRepository _repository;

        public BallService(IBallRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<IBall> GetBalls()
        {
            return _repository.GetInitialBalls();
        }

        public void UpdatePositions(
                                    IEnumerable<IBall> balls,
                                    double dt,
                                    double width,
                                    double height)
        {
            foreach (var ball in balls)
            {
                double newX = ball.X + ball.VX * dt;
                double newY = ball.Y + ball.VY * dt;

                // LEFT
                if (newX < 0)
                {
                    ball.X = 0;
                    ball.VX = 0;
                }
                // RIGHT
                else if (newX + ball.Radius * 2 > width)
                {
                    ball.X = width - ball.Radius * 2;
                    ball.VX = 0;
                }
                else
                {
                    ball.X = newX;
                }

                // TOP
                if (newY < 0)
                {
                    ball.Y = 0;
                    ball.VY = 0;
                }
                // BOTTOM
                else if (newY + ball.Radius * 2 > height)
                {
                    ball.Y = height - ball.Radius * 2;
                    ball.VY = 0;
                }
                else
                {
                    ball.Y = newY;
                }
            }
        }
    }
}