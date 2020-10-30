using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Validation
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ValidatorTests
    {
        private readonly HashSet<Direction> allLinks = new HashSet<Direction> { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };
        
        // Basic Validator tests with empty options

        [Test]
        public void Validate_NoCodeNoLinks_NoErrors()
        {
            var code = new List<Command>();
            var links = new HashSet<Direction>();

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void Validate_NoCodeAllLinks_NoErrors()
        {
            var code = new List<Command>();
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        // All the operators that take no parameters
        // must have no parameters

        [Test]
        [TestCase(Instruction.NOP)]
        [TestCase(Instruction.SWP)]
        [TestCase(Instruction.SAV)]
        [TestCase(Instruction.NEG)]
        public void Validate_Parameterless_NoErrors(Instruction ins)
        {

            var code = new List<Command> { new Command(ins, 0) };
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        [TestCase(Instruction.NOP, "MOV")]
        [TestCase(Instruction.SWP, "MOV")]
        [TestCase(Instruction.SAV, "MOV")]
        [TestCase(Instruction.NEG, "MOV")]
        public void Validate_ParameterlessToken_InvalidTokenError(Instruction ins, string value)
        {

            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, value)) };
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var error = errors[0];

            Assert.AreEqual(0, error.line);
            Assert.AreEqual(0, error.token);
        }

        [Test]
        [TestCase(Instruction.NOP, "MOV", "ADD")]
        [TestCase(Instruction.SWP, "MOV", "ADD")]
        [TestCase(Instruction.SAV, "MOV", "ADD")]
        [TestCase(Instruction.NEG, "MOV", "ADD")]
        public void Validate_ParameterlessDoubleToken_InvalidTokenErrors(Instruction ins, string value, string secondary)
        {

            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, value), new Parameter(Param.Text, secondary)) };
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(2, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);

            var e2 = errors[1];
            Assert.AreEqual(0, e2.line);
            Assert.AreEqual(1, e2.token);
        }
    }
}
