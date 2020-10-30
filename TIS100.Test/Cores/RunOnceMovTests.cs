using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceMovTests
    {
        [Test]
        public void RunOnce_MovNumberACC_UpdatesACC()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV 8 ACC"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(8, core.ACC);
        }

        [Test]
        public void RunOnce_MovNegativeNumberACC_UpdatesACC()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV -8 ACC"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(-8, core.ACC);
        }

        [Test]
        public void RunOnce_MovACCtoACC_UpdatesACC()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 3
MOV ACC ACC"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_MovACCtoUP_Writes()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });

            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 3
MOV ACC UP"));

            core.SetCommands(cmds);

            core.Step();
            core.Step();
            var read = core.GetDirection(Direction.UP, out var result);

            Assert.IsTrue(read);
            Assert.AreEqual(3, result);

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_MovUPtoACC_WithValue_UpdatesACC()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP ACC"));

            core.SetCommands(cmds);

            core.SetDirection(Direction.UP, 3);

            core.Step();

            Assert.AreEqual(3, core.ACC);
            Assert.AreEqual(1, core.PC);
        }

        [Test]
        public void RunOnce_MovUPtoACC_NoValue_Waits()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP ACC"));

            core.SetCommands(cmds);

            core.Step();

            Assert.AreEqual(0, core.ACC);
            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void RunOnce_MovUPtoACC_SetAfter_UpdatesACC()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP ACC"));

            core.SetCommands(cmds);

            core.Step();
            core.SetDirection(Direction.UP, 3);
            core.Step();

            Assert.AreEqual(3, core.ACC);
            Assert.AreEqual(1, core.PC);
        }

        [Test]
        public void RunOnce_MovUPtoACC_ClearsAfterRead_UpdatesACC()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP ACC"));

            core.SetCommands(cmds);

            core.Step();
            core.SetDirection(Direction.UP, 3);
            core.Step();
            core.Step();

            Assert.AreEqual(3, core.ACC);
            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void RunOnce_MovUPtoNIL_ClearsAfterRead_Discards()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP NIL"));

            core.SetCommands(cmds);

            core.Step();
            core.SetDirection(Direction.UP, 3);
            core.Step();
            core.Step();

            Assert.AreEqual(0, core.ACC);
            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void RunOnce_MovUPtoAcc_Twice_UpdatesACC()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.UP });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV UP ACC"));

            core.SetCommands(cmds);

            core.Step();
            core.SetDirection(Direction.UP, 3);
            core.Step();
            Assert.AreEqual(3, core.ACC);

            core.SetDirection(Direction.UP, 4);
            core.Step();
            Assert.AreEqual(4, core.ACC);
        }


        [Test]
        public void RunOnce_MovNumbertoLEFT_Read_WritesLeft()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.LEFT });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV 5 LEFT"));

            core.SetCommands(cmds);

            core.Step();
            var read = core.GetDirection(Direction.LEFT, out var result);

            Assert.AreEqual(1, core.PC);
            Assert.IsTrue(read);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void RunOnce_MovNumbertoLEFT_Blocked_Waits()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.LEFT });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV 5 LEFT
ADD 1"));

            core.SetCommands(cmds);
            core.Step();

            Assert.AreEqual(1, core.PC);
            Assert.AreEqual(0, core.ACC);
        }

        [Test]
        public void RunOnce_MovNumbertoLEFT_UnBlocked_Continues()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.LEFT });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV 5 LEFT
ADD 7"));

            core.SetCommands(cmds);
            core.Step();
            core.Step();
            Assert.AreEqual(0, core.ACC);
            core.Step();
            Assert.AreEqual(0, core.ACC);
            var read = core.GetDirection(Direction.LEFT, out var result);
            core.Step();

            Assert.AreEqual(2, core.PC);
            Assert.AreEqual(7, core.ACC);
        }

        [Test]
        public void RunOnce_MovNumbertoACC_UnBlocked_NoRead()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.LEFT });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV 5 ACC
ADD 3"));

            core.SetCommands(cmds);
            core.Step();
            var read = core.GetDirection(Direction.LEFT, out var result);
            core.Step();

            Assert.IsFalse(read);
            Assert.AreEqual(0, result);

            Assert.AreEqual(8, core.ACC);
        }

        [Test]
        public void RunOnce_MovLeftToRight_UnBlocked_Reads()
        {
            var core = new Core();
            core.SetDirections(new List<Direction> { Direction.LEFT, Direction.RIGHT });
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"MOV LEFT RIGHT"));

            core.SetCommands(cmds);

            core.SetDirection(Direction.LEFT, 8);
            core.Step();
            var read = core.GetDirection(Direction.RIGHT, out var result);
            core.Step();

            Assert.IsTrue(read);
            Assert.AreEqual(8, result);

            Assert.AreEqual(0, core.ACC);
        }
    }
}
