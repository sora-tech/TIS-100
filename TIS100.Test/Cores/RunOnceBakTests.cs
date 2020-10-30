using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RunOnceBakTests
    {

        [Test]
        public void RunOnce_SAV1_Saves()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
SAV"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(1, core.ACC);
            Assert.AreEqual(1, core.BAK);
        }

        [Test]
        public void RunOnce_SAV2_Overwrites()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
SAV
ADD 3
SAV"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(4, core.ACC);
            Assert.AreEqual(4, core.BAK);
        }

        [Test]
        public void RunOnce_SWP1_Swaps()
        {
            var core = new Core();
            List<Command> cmds = Parser.Instruct(Lexer.Lex(@"ADD 1
SWP"));

            core.SetCommands(cmds);

            core.RunOnce();

            Assert.AreEqual(0, core.ACC);
            Assert.AreEqual(1, core.BAK);
        }
    }
}
