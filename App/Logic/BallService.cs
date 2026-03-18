using System.Collections.Generic;
using App.Data;

namespace App.Logic
{
    public class BallService
    {
        private readonly BallRepository _repository;

        public BallService(BallRepository repository)
        {
            _repository = repository;
        }

        public List<Ball> GetBalls()
        {
            return _repository.GetInitialBalls();
        }

        public void UpdatePositions(List<Ball> balls, double dt)
        {
            foreach (var ball in balls)
            {
                ball.X += ball.VX * dt;
                ball.Y += ball.VY * dt;
            }
        }
    }
}