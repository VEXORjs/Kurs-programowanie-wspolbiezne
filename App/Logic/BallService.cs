using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using App.Data;

namespace App.Logic
{
    public class BallService : IBallService
    {
        private readonly IBallRepository _repository;
        private readonly ILogger _logger;

        public BallService(
            IBallRepository repository,
            ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IReadOnlyList<IBall> GetBalls(
            int count,
            double width,
            double height)
        {
            return _repository.GetInitialBalls(count, width, height);
        }

        public void UpdatePositions(
            IEnumerable<IBall> balls,
            double dt,
            double width,
            double height)
        {
            var ballList = balls.ToList();

            // =========================
            // 1. UPDATE POSITIONS (parallel + per-ball lock)
            // =========================
            Parallel.ForEach(ballList, ball =>
            {
                lock (ball)
                {
                    ball.Move(dt);
                    HandleBoundaries(ball, width, height);

                    var s = ball.GetState();
                    _logger.Log($"Ball X={s.X:F2} Y={s.Y:F2}");
                }
            });

            // =========================
            // 2. COLLISIONS (ordered locking)
            // =========================
            for (int i = 0; i < ballList.Count; i++)
            {
                for (int j = i + 1; j < ballList.Count; j++)
                {
                    var a = ballList[i];
                    var b = ballList[j];

                    var first =
                        RuntimeHelpers.GetHashCode(a) <
                        RuntimeHelpers.GetHashCode(b)
                            ? a
                            : b;

                    var second = first == a ? b : a;

                    lock (first)
                        lock (second)
                        {
                            ResolveCollision(a, b);
                        }
                }
            }
        }

        private void HandleBoundaries(
            IBall ball,
            double width,
            double height)
        {
            var state = ball.GetState();

            if (state.X < 0)
            {
                ball.SetPosition(0, state.Y);
                ball.BounceX();
            }

            if (state.X + ball.Radius * 2 > width)
            {
                ball.SetPosition(width - ball.Radius * 2, state.Y);
                ball.BounceX();
            }

            if (state.Y < 0)
            {
                ball.SetPosition(state.X, 0);
                ball.BounceY();
            }

            if (state.Y + ball.Radius * 2 > height)
            {
                ball.SetPosition(state.X, height - ball.Radius * 2);
                ball.BounceY();
            }
        }

        private void ResolveCollision(IBall a, IBall b)
        {
            var sa = a.GetState();
            var sb = b.GetState();

            double dx = sb.X - sa.X;
            double dy = sb.Y - sa.Y;

            double dist = Math.Sqrt(dx * dx + dy * dy);
            if (dist == 0) return;

            if (dist < a.Radius + b.Radius)
            {
                double nx = dx / dist;
                double ny = dy / dist;

                double p =
                    2 * (
                        sa.VX * nx +
                        sa.VY * ny -
                        sb.VX * nx -
                        sb.VY * ny
                    ) / (a.Mass + b.Mass);

                a.ResolveCollision(nx, ny, p, b.Mass);
                b.ResolveCollision(-nx, -ny, p, a.Mass);

                double overlap = (a.Radius + b.Radius) - dist;

                if (overlap > 0)
                {
                    double correction = overlap / 2;

                    a.SetPosition(sa.X - correction * nx, sa.Y - correction * ny);
                    b.SetPosition(sb.X + correction * nx, sb.Y + correction * ny);
                }
            }
        }
    }
}