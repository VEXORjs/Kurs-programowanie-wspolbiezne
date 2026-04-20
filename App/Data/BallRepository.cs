using System.Collections.Generic;

namespace App.Data
{
    public class BallRepository : IBallRepository
    {
        public IReadOnlyList<IBall> GetInitialBalls()
        {
            return new List<IBall>
            {
                new Ball { X = 700,  Y = 60,  VX = 50,  VY = 0, Radius = 20 },
                new Ball { X = 180, Y = 90,  VX = 0, VY = 30, Radius = 16 },
                new Ball { X = 320, Y = 150, VX = -20,  VY = 0, Radius = 24 },
                new Ball { X = 220, Y = 200, VX = 0,  VY = -10, Radius = 20 }
            };
        }
    }
}