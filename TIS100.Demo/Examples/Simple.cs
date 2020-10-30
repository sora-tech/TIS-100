using System;
using System.Collections.Generic;
using TIS100.Types;

namespace TIS100.Demo.Examples
{
    static class Simple
    {
        // Loads a single core with an example program
        // Stores the value of 8 in memory
        // Passes value from LEFT to RIGHT
        // Discards values from UP

        public static void Run()
        {
            Console.WriteLine("# Demo");
            Console.WriteLine();

            const string code = 
@"MOV 8, ACC
MOV LEFT, RIGHT
MOV UP, NIL";

            Console.WriteLine($"{code}");
            Console.WriteLine();

            var board = new Board(1, 1);
            board.Fill();

            board.Load(0, 0, code);

            var leftInput = board.AddInput(-1, 0, Direction.RIGHT, new Queue<int>(new List<int> { 1, 2, 3 }));
            var upInput = board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 5, 6, 7 }));

            Console.WriteLine("Input Left: " + string.Join(",", board.GetInput(leftInput).values.ToArray()));
            Console.WriteLine("Input Up: " + string.Join(",", board.GetInput(upInput).values.ToArray()));

            var rightOutput = board.AddOutput(1, 0, Direction.LEFT);

            while (board.GetOutput(rightOutput).values.Count != 3)
            {
                board.Step();
            }

            Console.WriteLine("Core ACC: " + board.Core(0, 0).ACC);
            Console.WriteLine("Output Right: " + string.Join(",", board.GetOutput(rightOutput).values.ToArray()));
            Console.WriteLine();
        }

    }
}
