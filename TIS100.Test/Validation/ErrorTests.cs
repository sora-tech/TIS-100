using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Validation
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ErrorTests
    {
        [Test]
        public void Error_HasLineNumber()
        {
            var command = new Command(Instruction.ADD, 5);

            var error = new Error(command, "", 0);

            Assert.AreEqual(5, error.line);
        }

        [Test]
        public void Error_HasCommand()
        {
            var command = new Command(Instruction.ADD, 5);

            var error = new Error(command, "", 0);

            Assert.AreEqual(command, error.command);
        }

        [Test]
        public void Error_HasMessage()
        {
            var command = new Command(Instruction.ADD, 5);

            var error = new Error(command, "invalid number", 1);

            Assert.AreEqual("invalid number", error.message);
        }

        [Test]
        public void Error_HasTokenCount()
        {
            var command = new Command(Instruction.ADD, 5);

            var error = new Error(command, "invalid number", 1);

            Assert.AreEqual(1, error.token);
        }
    }
}
