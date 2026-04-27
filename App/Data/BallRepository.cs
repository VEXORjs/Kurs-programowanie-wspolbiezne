using System;
using System.Collections.Generic;

namespace App.Data
{
    public class BallRepository : IBallRepository
    {
        public IReadOnlyList<IBall> GetInitialBalls(int count, double width, double height)
        {
            var rand = new Random();
            var balls = new List<IBall>();

            const double radius = 25;
            const int maxAttempts = 1000;

            for (int i = 0; i < count; i++)
            {
                int attempts = 0;
                bool positionFound = false;

                double x = 0;
                double y = 0;

                double VX = 0;
                double VY = 0;

                while (!positionFound && attempts < maxAttempts)
                {
                    attempts++;

                    x = rand.Next((int)radius, (int)(width - radius));
                    y = rand.Next((int)radius, (int)(height - radius));
                    VX = rand.Next(-100, 100);
                    VY = rand.Next(-100, 100);

                    positionFound = true;

                    foreach (var b in balls)
                    {
                        double dx = b.X - x;
                        double dy = b.Y - y;
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        if (distance < radius * 2)
                        {
                            positionFound = false;
                            break;
                        }
                    }
                }

                // fallback
                if (!positionFound)
                {
                    x = rand.Next((int)radius, (int)(width - radius));
                    y = rand.Next((int)radius, (int)(height - radius));
                    VX = rand.Next(-100, 100);
                    VY = rand.Next(-100, 100);
                }

                if (i == 0)
                {
                    VX = 1000;
                    VY = 1000;
                }

                balls.Add(new Ball
                {
                    X = x,
                    Y = y,
                    VX = VX,
                    VY = VY,
                    Radius = radius,
                    Mass = 1.0
                });
            }

            return balls;
        }
    }
}