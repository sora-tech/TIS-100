using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceJumpTests
    {

        [Test]
        public void RunOnce_JMP_AdvancePCToLabel()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" JMP A
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(2, core.PC);
        }

        [Test]
        public void RunOnce_JMP_RunOncesAfter()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" JMP A
A:
ADD 1"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_JMPOver_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" JMP A
ADD 1
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(0, core.ACC);
        }


        [Test]
        public void RunOnce_JMPTwoLabels_AdvancePCToLabel()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" B:
JMP A
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(3, core.PC);
        }

        [Test]
        public void RunOnce_JEZZero_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" JEZ A
ADD 1
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(0, core.ACC);
        }

        [Test]
        public void RunOnce_JEZOne_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@" ADD 1
JEZ A
ADD 1
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_JNZZero_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
JNZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_JNZOne_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"
JNZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_JGZZero_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JGZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_JGZOne_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
JGZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_JGZNegative_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"SUB 1
JGZ A
SUB 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(-3, core.ACC);
        }


        [Test]
        public void RunOnce_JLZZero_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JLZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_JLZOne_NotRunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
JLZ A
ADD 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_JLZNegative_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"SUB 1
JLZ A
SUB 2
A:"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(-1, core.ACC);
        }

        [Test]
        public void RunOnce_JLZNegativeContinues_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"SUB 1
JLZ A
SUB 2
A:
SUB 5"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(-6, core.ACC);
        }


        [Test]
        public void RunOnce_JROZero_RunOnced()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JRO 0"));

            core.SetCommands(cmds);

            core.Step();

            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void RunOnce_JROOne_RunOnceNext()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JRO 1
ADD 1
ADD 3"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(4, core.ACC);
        }

        [Test]
        public void RunOnce_JROTwo_RunOnceSkip()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JRO 2
ADD 1
ADD 3"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_JRONegative_RunOncePrevious()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
JRO -1
ADD 3"));

            core.SetCommands(cmds);

            core.Step();    // ADD 1
            Assert.AreEqual(1, core.PC);

            core.Step();    // JRO -1
            Assert.AreEqual(0, core.PC);

            core.Step();    // ADD 1
            Assert.AreEqual(1, core.PC);

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_JROACC_RunOnceSkip()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 2
JRO ACC
ADD 1
ADD 3"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(5, core.ACC);
        }

        [Test]
        public void RunOnce_JROUP_WithValue_RunOnceSkip()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"JRO UP
ADD 1
ADD 3"));

            core.SetCommands(cmds);

            core.SetDirection(Direction.UP, 2);
            core.Step();
            core.Step();

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_JROUP_NoValue_Waits()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 5
JRO UP
ADD 3"));

            core.SetCommands(cmds);

            core.Step();
            core.Step();

            Assert.AreEqual(5, core.ACC);
            Assert.AreEqual(1, core.PC);
        }
    }
}
