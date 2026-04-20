using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public interface IBallService
    {
        IReadOnlyList<Ball> GetBalls();
        void UpdatePositions(IEnumerable<Ball> balls, double dt);
    }
}