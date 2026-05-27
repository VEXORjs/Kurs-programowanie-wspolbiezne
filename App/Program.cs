using System;
using System.Windows;

namespace App
{
    public partial class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App.Presentation.App();
            app.InitializeComponent();
            //concurrent programming
            app.Run();
        }
    }
}
