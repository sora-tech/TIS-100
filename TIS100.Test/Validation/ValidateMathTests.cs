using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Validation
{
    // Two maths operators ADD and SUB work the same way
    // They must have a single parameter that is a valid
    // number (number, direction, acc)

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ValidateMathTests
    {
        private readonly HashSet<Direction> allLinks = new HashSet<Direction> { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateMaths_SecondToken_InvalidTokenError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(0), new Parameter(Direction.UP)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateAdd_FirstDestInvalid_InvalidDirectionError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Direction.UP)) };
            var links = new HashSet<Direction> { Direction.DOWN };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateAdd_FirstTestInvalid_InvalidTokenError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, "MOV")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateAdd_ValidNumber_NoErrors(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(9)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateAdd_ValidLink_NoErrors(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Direction.UP)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        [TestCase(Instruction.ADD)]
        [TestCase(Instruction.SUB)]
        public void ValidateAdd_ValidAcc_NoErrors(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.ACC)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }
    }
}
