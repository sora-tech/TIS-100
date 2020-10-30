using System;
using TIS100.Demo.Examples;

namespace TIS100.Demo
{
    // An example console application to run examples
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "TIS-100";

            Simple.Run();

            Doubler.Run();

            Sorter.Run();

            Console.ReadLine();
        }
    }
}
