using System.Collections.Generic;

namespace App.Data
{
    public class BallRepository
    {
        public List<Ball> GetInitialBalls()
        {
            return new List<Ball>
            {
                new Ball { X = 0, Y = 0, VX = 0, VY = 0, Radius = 10 },
                new Ball { X = 10, Y = 5, VX = 0, VY = 0, Radius = 5 },
                new Ball { X = -3, Y = 7, VX = 0, VY = 0, Radius = 20 }
            };
        }
    }
}