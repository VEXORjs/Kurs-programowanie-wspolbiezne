using System;
using App.Presentation.ViewModel;

namespace App.Presentation.View
{
    public class ConsoleView
    {
        public void Run()
        {
            var vm = new BallViewModel();

            Console.WriteLine("Initial ball positions:");

            foreach (var ball in vm.Balls)
            {
                Console.WriteLine($"X: {ball.X}, Y: {ball.Y}");
            }

            Console.ReadKey();
        }
    }
}