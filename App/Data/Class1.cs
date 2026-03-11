using System;
using System.Collections.Generic;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            List<string> lista =  new List<string>();
            lista.Add("Jabłko");
            lista.Add("Gruszka");
            lista.Add("Banan");

            for (int i = 0; i <lista.Count; i++) {
                Console.WriteLine(lista[i]);
            }

            Console.ReadKey();
        }
    }
}