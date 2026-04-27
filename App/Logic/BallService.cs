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

        public IReadOnlyList<IBall> GetBalls(int count, double width, double height)
        {
            return _repository.GetInitialBalls(count, width, height);
        }

        public void UpdatePositions(IEnumerable<IBall> balls, double dt, double width, double height)
        {
            var ballList = balls.ToList();

            Parallel.ForEach(ballList, ball =>
            {
                lock (ball.Lock)
                {
                    ball.X += ball.VX * dt;
                    ball.Y += ball.VY * dt;

                    // LEFT
                    if (ball.X < 0)
                    {
                        ball.X = 0;
                        ball.VX *= -1;
                    }

                    // RIGHT
                    if (ball.X + ball.Radius * 2 > width)
                    {
                        ball.X = width - ball.Radius * 2;
                        ball.VX *= -1;
                    }

                    // TOP
                    if (ball.Y < 0)
                    {
                        ball.Y = 0;
                        ball.VY *= -1;
                    }

                    // BOTTOM
                    if (ball.Y + ball.Radius * 2 > height)
                    {
                        ball.Y = height - ball.Radius * 2;
                        ball.VY *= -1;
                    }
                }
            });

            // kolizje (sekcja krytyczna!)
            for (int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    var a = ballList[i];
                    var b = ballList[j];

                    lock (a.Lock)
                        lock (b.Lock)
                        {
                            ResolveCollision(a, b);
                        }
                }
            }
        }
        private void ResolveCollision(IBall a, IBall b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist == 0) return;

            if (dist < a.Radius + b.Radius)
            {
                double nx = dx / dist;
                double ny = dy / dist;

                double p = 2 * (
                    a.VX * nx + a.VY * ny -
                    b.VX * nx - b.VY * ny
                ) / (a.Mass + b.Mass);

                a.VX -= p * b.Mass * nx;
                a.VY -= p * b.Mass * ny;

                b.VX += p * a.Mass * nx;
                b.VY += p * a.Mass * ny;

                double overlap = (a.Radius + b.Radius) - dist;

                if (overlap > 0)
                {
                    double correction = overlap / 2;

                    a.X -= correction * nx;
                    a.Y -= correction * ny;

                    b.X += correction * nx;
                    b.Y += correction * ny;
                }
            }
        }
    }
}