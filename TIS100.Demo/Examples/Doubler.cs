using System;
using System.Collections.Generic;
using TIS100.Types;

namespace TIS100.Demo.Examples
{
    static class Doubler
    {
        // Loads two cores with a doubler program
        // Input on the LEFT and output on RIGHT

        public static void Run()
        {
            Console.WriteLine("# Doubler");
            Console.WriteLine();

            const string code = 
@"MOV LEFT ACC
ADD ACC
MOV ACC RIGHT";

            Console.WriteLine($"{code}");
            Console.WriteLine();

            var board = new Board(2, 1);
            board.Fill();

            board.Load(0, 0, code);
            board.Load(1, 0, code);

            var leftInput = board.AddInput(-1, 0, Direction.RIGHT, new Queue<int>(new List<int> { 1, 2, 3 }));

            Console.WriteLine("Input: " + string.Join(",", board.GetInput(leftInput).values.ToArray()));

            var rightOutput = board.AddOutput(2, 0, Direction.LEFT);

            while (board.GetOutput(rightOutput).values.Count != 3)
            {
                board.Step();
            }

            Console.WriteLine("Output: " + string.Join(",", board.GetOutput(rightOutput).values.ToArray()));
            Console.WriteLine();
        }
    }
}
