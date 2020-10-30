using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace TIS100.Test.Cores
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class LoadTests
    {
        [Test]
        public void SetCommands_DoesNotThrow()
        {
            const string code = @"";

            var commands = Parser.Instruct(Lexer.Lex(code));

            var core = new Core();

            Assert.DoesNotThrow(() => core.SetCommands(commands));
        }

        [Test]
        public void ValidateCommands_NoCode_Valid()
        {
            const string code = @"";

            var commands = Parser.Instruct(Lexer.Lex(code));

            var core = new Core();
            core.SetCommands(commands);

            var errors = core.Validate();
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateCommands_InvalidCode_HasErrors()
        {
            const string code = @"MOV MOV MOV";

            var commands = Parser.Instruct(Lexer.Lex(code));

            var core = new Core();
            core.SetCommands(commands);

            var errors = core.Validate();
            Assert.IsNotEmpty(errors);
        }

        [Test]
        public void ValidateCommands_ValidCode_Valid()
        {
            const string code = @"ADD 1";

            var commands = Parser.Instruct(Lexer.Lex(code));

            var core = new Core();
            core.SetCommands(commands);

            var errors = core.Validate();
            Assert.IsEmpty(errors);
        }
    }
}
