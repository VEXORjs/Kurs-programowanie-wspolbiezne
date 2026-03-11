using System;
using ViewModel;

namespace View
{
    public class ConsoleView
    {
        public void Display()
        {
            var vm = new FruitViewModel();
            foreach (var fruit in vm.Fruits)
            {
                Console.WriteLine(fruit.Name);
            }
        }
    }
}