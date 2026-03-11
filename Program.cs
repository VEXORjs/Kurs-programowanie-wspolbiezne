using System;
using View;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            var view = new ConsoleView();
            view.Display();

            Console.ReadKey();
        }
    }
}