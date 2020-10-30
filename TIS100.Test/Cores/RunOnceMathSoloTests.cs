using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceMathSoloTests
    {

        [Test]
        public void RunOnce_AddOne_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("ADD 1"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_AddTwice_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex(@"ADD 1
ADD 7"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(8, core.ACC);
        }

        [Test]
        public void RunOnce_AddNegativeOne_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("ADD -3"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(-3, core.ACC);
        }

        [Test]
        public void RunOnce_AddACC_UpdatesACC()
        {
            var core = new Core();
            List<Command> addACC = Parser.Instruct(Lexer.Lex(@"ADD 1
ADD ACC"));

            core.SetCommands(addACC);

            core.RunOnce();

            Assert.AreEqual(2, core.ACC);
        }

        [Test]
        public void RunOnce_AddNIL_NoChangeACC()
        {
            var core = new Core();
            List<Command> addACC = Parser.Instruct(Lexer.Lex(@"ADD 1
ADD NIL"));

            core.SetCommands(addACC);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_SubOne_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("SUB 1"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(-1, core.ACC);
        }

        [Test]
        public void RunOnce_SubTwice_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex(@"SUB 1
SUB 7"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(-8, core.ACC);
        }

        [Test]
        public void RunOnce_SubNegativeOne_UpdatesACC()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("SUB -3"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(3, core.ACC);
        }

        [Test]
        public void RunOnce_SubACC_UpdatesACC()
        {
            var core = new Core();
            List<Command> addACC = Parser.Instruct(Lexer.Lex(@"SUB 2
SUB ACC"));

            core.SetCommands(addACC);

            core.RunOnce();

            Assert.AreEqual(0, core.ACC);
        }

        [Test]
        public void RunOnce_SubNIL_NoChangeACC()
        {
            var core = new Core();
            List<Command> addACC = Parser.Instruct(Lexer.Lex(@"ADD 1
SUB NIL"));

            core.SetCommands(addACC);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_NegPositive_UpdatesACCNegative()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex(@"ADD 1
NEG"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(-1, core.ACC);
        }

        [Test]
        public void RunOnce_NegNegative_UpdatesACCPositive()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex(@"SUB 1
NEG"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
        }

        [Test]
        public void RunOnce_NegZero_ACCRemainsZero()
        {
            var core = new Core();
            List<Command> add1 = Parser.Instruct(Lexer.Lex("NEG"));

            core.SetCommands(add1);

            core.RunOnce();

            Assert.AreEqual(0, core.ACC);
        }
    }
}
