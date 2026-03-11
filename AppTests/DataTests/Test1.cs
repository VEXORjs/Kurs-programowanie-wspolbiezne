using Data;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Collections.Generic;

namespace DataTests
{
    [TestClass]
    public class FruitRepositoryTests
    {
        [TestMethod]
        public void GetFruits_ReturnsThreeFruits()
        {
            var repo = new FruitRepository();
            List<Fruit> fruits = repo.GetFruits();

            Assert.AreEqual(3, fruits.Count);
            Assert.AreEqual("Jabłko", fruits[0].Name);
        }
    }
}