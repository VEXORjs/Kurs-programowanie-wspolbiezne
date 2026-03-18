using App.Presentation.View;

namespace App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var view = new ConsoleView();
            view.Run();
        }
    }
}