using System;
using System.Collections.Generic;
using TIS100.Types;

namespace TIS100.Demo.Examples
{
    static class Sorter
    {
        // Loads a single core with a sorting program
        // Reads the value from UP and writes to
        // Right if positive of Left if negative
        // discards zeros

        public static void Run()
        {
            Console.WriteLine("# Sorter:");
            Console.WriteLine();

            const string code = 
@"START:
MOV UP ACC
 JGZ POSITIVE
 JLZ NEGATIVE
JMP START
POSITIVE:
 MOV ACC RIGHT
 JMP START
NEGATIVE:
 MOV ACC LEFT
 JMP START";

            Console.WriteLine($"{code}");
            Console.WriteLine();

            var board = new Board(1, 1);
            board.Fill();

            board.Load(0, 0, code);

            var upInput = board.AddInput(0, -1, Direction.DOWN, new Queue<int>(new List<int> { 5, -6, 0, -2, 3 }));

            Console.WriteLine("Input: " + string.Join(",", board.GetInput(upInput).values.ToArray()));

            var rightOutput = board.AddOutput(1, 0, Direction.LEFT);
            var leftOutput = board.AddOutput(-1, 0, Direction.RIGHT);

            while (board.GetOutput(rightOutput).values.Count != 2)
            {
                board.Step();
            }

            Console.WriteLine("Output Left : " + string.Join(",", board.GetOutput(leftOutput).values.ToArray()));
            Console.WriteLine("Output Right: " + string.Join(",", board.GetOutput(rightOutput).values.ToArray()));

            Console.WriteLine();
        }

    }
}
