using System;

namespace View
{
    static class Program
    {
        static void Main(string[] args)
        {
            var view = new ConsoleView();
            view.Display();
            Console.ReadKey();
        }
    }
}