using System;
using System.Windows;

namespace App
{
    public partial class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new Presentation.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}