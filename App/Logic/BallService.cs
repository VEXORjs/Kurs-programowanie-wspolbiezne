using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public class BallService : IBallService
    {
        private readonly IBallRepository _repository;

        public BallService(IBallRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<Ball> GetBalls()
        {
            return _repository.GetInitialBalls();
        }

        public void UpdatePositions(IEnumerable<Ball> balls, double dt)
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VX * dt;
                ball.Y += ball.VY * dt;
            }
        }
    }
}