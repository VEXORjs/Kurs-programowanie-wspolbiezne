using System.Collections.Generic;
using Data;
using Model;

namespace Logic
{
    public class FruitService
    {
        private readonly FruitRepository _repository = new FruitRepository();

        public List<Fruit> GetAllFruits()
        {
            return _repository.GetFruits();
        }
    }
}