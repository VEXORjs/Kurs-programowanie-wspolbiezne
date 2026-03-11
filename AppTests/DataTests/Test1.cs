using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace DataTests
{

    [TestClass]
    public class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var obj = new Klasa1(1);
            Assert.AreEqual(1, obj.A);
        }
    }
}