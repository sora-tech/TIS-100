using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    // Large scale integration tests of multiple instructions
    // on a single core
    [TestFixture]
    [Category("Integration")]
    [ExcludeFromCodeCoverage]
    public class CompleteTests
    {
        [Test]
        public void Complete_MovExample_Runs()
        {
            const string input = @"MOV 8, ACC
MOV LEFT, RIGHT
MOV UP, NIL";

            var commands = Parser.Instruct(Lexer.Lex(input));

            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP });
            core.SetCommands(commands);

            var errors = core.Validate();

            Assert.IsEmpty(errors);

            core.Step();    // MOV 8 ACC
            Assert.AreEqual(8, core.ACC);

            core.SetDirection(Direction.LEFT, 5);
            core.Step();    // MOV LEFT RIGHT
            var readRight = core.GetDirection(Direction.RIGHT, out var right);

            Assert.IsTrue(readRight);
            Assert.AreEqual(5, right);

            core.SetDirection(Direction.UP, 7);
            Assert.DoesNotThrow(() => core.Step());    //MOV UP NIL
        }

        [Test]
        public void Complete_ExampleOne_Runs()
        {
            const string input = @"MOV LEFT ACC
ADD ACC
MOV ACC RIGHT";

            var commands = Parser.Instruct(Lexer.Lex(input));

            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP });
            core.SetCommands(commands);

            var errors = core.Validate();

            Assert.IsEmpty(errors);

            core.SetDirection(Direction.LEFT, 8);
            core.Step();    // MOV LEFT ACC
            
            Assert.AreEqual(8, core.ACC);

            core.Step();    //ADD ACC
            Assert.AreEqual(16, core.ACC);

            core.Step();    //MOV ACC RIGHT
            var readRight = core.GetDirection(Direction.RIGHT, out var right);

            Assert.IsTrue(readRight);
            Assert.AreEqual(16, right);
        }


        [Test]
        public void Complete_ExampleTwo_Positive_Runs()
        {
            const string input = @"START:
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

            const int val = 3;

            var commands = Parser.Instruct(Lexer.Lex(input));

            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP });
            core.SetCommands(commands);

            var errors = core.Validate();

            Assert.IsEmpty(errors);

            core.Step();    // START
            
            core.SetDirection(Direction.UP, val);
            core.Step();    //MOV UP ACC
            Assert.AreEqual(val, core.ACC);

            core.Step();    //JGZ POSITIVE
            Assert.AreEqual(5, core.PC);
            core.Step();    //POSITIVE
            core.Step();    //MOV ACC RIGHT
            var readRight = core.GetDirection(Direction.RIGHT, out var right);

            Assert.IsTrue(readRight);
            Assert.AreEqual(val, right);

            core.Step();    //JGZ START
            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void Complete_ExampleTwo_Negative_Runs()
        {
            const string input = @"START:
 MOV UP, ACC
 JGZ POSITIVE
 JLZ NEGATIVE
 JMP START
POSITIVE:
 MOV ACC RIGHT
 JMP START
NEGATIVE:
 MOV ACC LEFT
 JMP START";

            const int val = -5;

            var commands = Parser.Instruct(Lexer.Lex(input));

            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP });
            core.SetCommands(commands);

            var errors = core.Validate();

            Assert.IsEmpty(errors);

            core.Step();    // START

            core.SetDirection(Direction.UP, val);
            core.Step();    //MOV UP ACC
            Assert.AreEqual(val, core.ACC);

            core.Step();    //JGZ POSITIVE
            core.Step();    //JLZ NEGATIVE

            core.Step();    //NEGATIVE
            core.Step();    //MOV ACC LEFT
            var readLeft = core.GetDirection(Direction.LEFT, out var left);

            Assert.IsTrue(readLeft);
            Assert.AreEqual(val, left);

            core.Step();    //JGZ START
            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void Complete_ExampleTwo_Zero_Runs()
        {
            const string input = @"START:
 MOV UP, ACC
 JGZ POSITIVE
 JLZ NEGATIVE
 JMP START
POSITIVE:
 MOV ACC, RIGHT
 JMP START
NEGATIVE:
 MOV ACC, LEFT
 JMP START";

            const int val = 0;

            var commands = Parser.Instruct(Lexer.Lex(input));

            var core = new Core();

            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT, Direction.UP });
            core.SetCommands(commands);

            var errors = core.Validate();

            Assert.IsEmpty(errors);

            core.Step();    // START

            core.SetDirection(Direction.UP, val);
            core.Step();    //MOV UP ACC
            Assert.AreEqual(val, core.ACC);

            core.Step();    //JGZ POSITIVE
            core.Step();    //JLZ NEGATIVE
            core.Step();    //JMP START

            Assert.AreEqual(0, core.PC);
        }
    }
}
