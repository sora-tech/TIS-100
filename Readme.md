# A TIS-100 Emulator

This project contains an emulator for the TIS-100 computer from the Zachtronic's game of the same name.
The goal of the game is to write programs in the assembly language provided that solve signal processing challenges.
These programs run on a TIS-100 computer made up of linked cores running in parallel.

If that sounds interesting to you I highly reccomend checking out TIS-100 as well as their other games.  If it sounds
terrifying and not fun at all I understand completely.

The goal of this project is to provide an emulator of TIS-100 that let you set any number of cores and run any program
on them.  I am basing the language on the definitions given by the game's manual and the layout and interactions from
the game itself.

An example program in TIS-100 assembly:

```
MOV LEFT, ACC
ADD ACC
MOV ACC, RIGHT
```

Running this program will:

1. Read the input to the left of the core and store it in the internal memory
2. Add the value of the internal memory to the internal memory
3. Write the value of the internal memory to the right output

Explaining the full system of the board is beyond the scope of this intro (and would spoil much of the game).

## Approaching the problem

I had already decided on using C# and TDD as a means to create the emulator however first I needed to isolate exactly
what creating an emulator entailed. The first breakdown is into two parts: the language and, the runtime.

Starting with the language gave way to the fairly obvious problem of writing an interpreter to make sense of
the plain text.  This I then broke down into two smaller problems: Lexing and Parsing.  Once those were completed I
moved to the runtime which could also be broken down into two areas: executing the code on a Core and, communication
between the Cores and the Inputs and Outputs.

Solving these problems using TDD was fairly straightforwards in combination with the manual that gave detailed
information on the expected behaviours.  In the solution it is possible to re-create my process by following the tests
in the order mentioned above and by working down the test file.  For more detailed information each class contains
an explanation alongside the code.

## Future additions

There are some components of the game involved in the later puzzles that are not included such as stack memory nodes.
Other extensions include the a save & load feature for boards which must be manually constructed, multi-threading 
to improve execution time or even network support for nodes to create massive boards with millions of nodes
making a TIS-100 supercomputer.
