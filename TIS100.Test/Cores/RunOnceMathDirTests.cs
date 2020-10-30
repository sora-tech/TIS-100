using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceMathDirTests
    {

        [Test]
        public void RunOnce_AddUp_WithValue_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("ADD UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.SetDirection(Direction.UP, 5);
            core.Step();

            Assert.AreEqual(5, core.ACC);
            Assert.AreEqual(1, core.PC);
        }

        [Test]
        public void RunOnce_AddUp_NoValue_Waits()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("ADD UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.Step();

            Assert.AreEqual(0, core.ACC);
            Assert.AreEqual(0, core.PC);
        }


        [Test]
        public void RunOnce_AddUp_WithValue_Clears()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("ADD UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.Step();
            core.SetDirection(Direction.UP, 5);
            core.Step();
            core.Step();

            Assert.AreEqual(5, core.ACC);
            Assert.AreEqual(0, core.PC);
        }


        [Test]
        public void RunOnce_SubUp_WithValue_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("SUB UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.SetDirection(Direction.UP, 5);
            core.Step();

            Assert.AreEqual(-5, core.ACC);
            Assert.AreEqual(1, core.PC);
        }

        [Test]
        public void RunOnce_SubUp_NoValue_Waits()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("SUB UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.Step();

            Assert.AreEqual(0, core.ACC);
            Assert.AreEqual(0, core.PC);
        }


        [Test]
        public void RunOnce_SubUp_WithValue_Clears()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("SUB UP"));

            core.SetDirections(new List<Direction> { Direction.UP });
            core.SetCommands(add1);

            core.Step();
            core.SetDirection(Direction.UP, 5);
            core.Step();
            core.Step();

            Assert.AreEqual(-5, core.ACC);
            Assert.AreEqual(0, core.PC);
        }
    }
}
