using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceTests
    {
        [Test]
        public void RunOnce_NoInstructions_DoesNotThrow()
        {
            var core = new Core();

            Assert.DoesNotThrow(() => core.RunOnce());
        }

        [Test]
        public void Step_NoInstructions_DoesNotThrow()
        {
            var core = new Core();

            Assert.DoesNotThrow(() => core.Step());
        }

        [Test]
        public void RunOnce_NoInstructions_NotAdvancePC()
        {
            var core = new Core();

            core.RunOnce();

            Assert.AreEqual(0, core.PC);
        }

        [Test]
        public void RunOnce_SingleNOP_AdvancePC()
        {
            var core = new Core();
            List<Command> nop = Parser.Instruct(Lexer.Lex("NOP"));

            core.SetCommands(nop);

            core.RunOnce();

            Assert.AreEqual(1, core.PC);
        }

        [Test]
        public void RunOnceTwice_SingleNOP_AdvancePC()
        {
            var core = new Core();
            List<Command> nop = Parser.Instruct(Lexer.Lex("NOP"));

            core.SetCommands(nop);

            core.RunOnce();
            core.RunOnce();

            Assert.AreEqual(1, core.PC);
        }


        [Test]
        public void StepTwice_SingleNOP_AdvancePC()
        {
            var core = new Core();
            List<Command> nop = Parser.Instruct(Lexer.Lex("NOP"));

            core.SetCommands(nop);

            core.Step();
            core.Step();

            Assert.AreEqual(1, core.PC);
        }
    }
}
