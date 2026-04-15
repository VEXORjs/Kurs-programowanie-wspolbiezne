using System.Collections.Generic;
using App.Data;
using App.Logic;

namespace App.Presentation.ViewModel
{
    public class BallViewModel
    {
        private readonly BallService _service;

        public List<Ball> Balls { get; private set; }


        public BallViewModel()
        {
            _service = new BallService(new BallRepository());
            Balls = _service.GetBalls();
        }
    }
}