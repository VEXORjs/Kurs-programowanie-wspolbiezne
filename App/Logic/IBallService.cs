using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public interface IBallService
    {
        IReadOnlyList<IBall> GetBalls(
            int count,
            double width,
            double height);

        void UpdatePositions(
            IEnumerable<IBall> balls,
            double dt,
            double width,
            double height);

        Task StartSimulationAsync(
            double width,
            double height,
            IEnumerable<IBall> balls,
            Action onTick,
            CancellationToken token);
    }
}