using System.Collections.Generic;
using App.Data;

namespace AppTests
{
    public class FakeBallRepository : IBallRepository
    {
        public IReadOnlyList<IBall> GetInitialBalls()
        {
            return new List<IBall>
            {
                new Ball { X = 0, Y = 0 }
            };
        }
    }
}