using Logic;
using Model;
using System.Collections.Generic;

namespace ViewModel
{
    public class FruitViewModel
    {
        private readonly FruitService _service = new FruitService();

        public List<Fruit> Fruits { get; }

        public FruitViewModel()
        {
            Fruits = _service.GetAllFruits();
        }
    }
}