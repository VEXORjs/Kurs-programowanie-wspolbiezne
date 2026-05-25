using System.Collections.Generic;

namespace App.Data
{
    public interface IBallRepository
    {
        IReadOnlyList<IBall> GetInitialBalls(
            int count,
            double width,
            double height);
    }
}