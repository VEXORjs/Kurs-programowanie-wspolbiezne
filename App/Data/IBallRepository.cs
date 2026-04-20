using System.Collections.Generic;

namespace App.Data
{
    public interface IBallRepository
    {
        IReadOnlyList<Ball> GetInitialBalls();
    }
}