using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public interface IBallService
    {
        IReadOnlyList<IBall> GetBalls();

        void UpdatePositions(
            IEnumerable<IBall> balls,
            double dt,
            double width,
            double height);
    }
}