using System.Collections.Generic;

namespace App.Data
{
    public class BallRepository : IBallRepository
    {
        public IReadOnlyList<Ball> GetInitialBalls()
        {
            return new List<Ball>
            {
                new Ball { X = 60,  Y = 60,  VX = 40,  VY = 25, Radius = 20 },
                new Ball { X = 180, Y = 90,  VX = -35, VY = 30, Radius = 16 },
                new Ball { X = 320, Y = 150, VX = 20,  VY = -28, Radius = 24 }
            };
        }
    }
}