using Model;
using System.Collections.Generic;

namespace Data
{
    public class FruitRepository
    {
        public List<Fruit> GetFruits()
        {
            return new List<Fruit>
            {
                new Fruit("Jabłko"),
                new Fruit("Gruszka"),
                new Fruit("Banan")
            };
        }
    }
}