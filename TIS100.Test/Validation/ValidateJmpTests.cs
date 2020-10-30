using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Validation
{
    // All Jump instructions except JRO use the same format
    // They must contain a single parameter which must
    // be a reference to an existing label
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ValidateJmpTests
    {
        private readonly HashSet<Direction> allLinks = new HashSet<Direction> { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };

        [Test]
        public void ValidateLabel_Unique_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.LBL, 0, new Parameter(Param.Text, "A")), new Command(Instruction.LBL, 1, new Parameter(Param.Text, "A")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(2, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(1, e1.line);
            Assert.AreEqual(0, e1.token);

            var e2 = errors[1];
            Assert.AreEqual(0, e2.line);
            Assert.AreEqual(0, e2.token);
        }

        [Test]
        [TestCase(Instruction.JMP)]
        [TestCase(Instruction.JEZ)]
        [TestCase(Instruction.JNZ)]
        [TestCase(Instruction.JGZ)]
        [TestCase(Instruction.JLZ)]
        public void ValidateJMP_NoToken_InvalidTokenError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        [TestCase(Instruction.JMP)]
        [TestCase(Instruction.JEZ)]
        [TestCase(Instruction.JNZ)]
        [TestCase(Instruction.JGZ)]
        [TestCase(Instruction.JLZ)]
        public void ValidateJMP_NonTextToken_InvalidTokenError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(0)) };

        var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        [TestCase(Instruction.JMP)]
        [TestCase(Instruction.JEZ)]
        [TestCase(Instruction.JNZ)]
        [TestCase(Instruction.JGZ)]
        [TestCase(Instruction.JLZ)]
        public void ValidateJMP_TwoTokens_InvalidTokenError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, "A"), new Parameter(Direction.UP)), new Command(Instruction.LBL, 1, new Parameter(Param.Text, "A")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }


        [Test]
        [TestCase(Instruction.JMP)]
        [TestCase(Instruction.JEZ)]
        [TestCase(Instruction.JNZ)]
        [TestCase(Instruction.JGZ)]
        [TestCase(Instruction.JLZ)]
        public void ValidateJMP_InvalidLabel_InvalidLabelError(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, "LBL")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        [TestCase(Instruction.JMP)]
        [TestCase(Instruction.JEZ)]
        [TestCase(Instruction.JNZ)]
        [TestCase(Instruction.JGZ)]
        [TestCase(Instruction.JLZ)]
        public void ValidateJMP_Valid_NoErrors(Instruction ins)
        {
            var code = new List<Command> { new Command(ins, 0, new Parameter(Param.Text, "A")), new Command(Instruction.LBL, 1, new Parameter(Param.Text, "A")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
            Assert.AreEqual(0, errors.Count);
        }


        // Jump Relative Offset requires a single parameter which
        // must be a valid number (number, acc or direction)

        [Test]
        public void ValidateJRO_NoToken_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.JRO, 0) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        public void ValidateJRO_SecondToken_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.JRO, 0, new Parameter(0), new Parameter(Param.Text, "A")) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        public void ValidateJRO_NegativeOffest_InvalidParameterError()
        {
            var code = new List<Command> { new Command(Instruction.JRO, 0, new Parameter(-100)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        public void ValidateJRO_PositiveOffestOverflow_InvalidParameterError()
        {
            var code = new List<Command> { new Command(Instruction.JRO, 0, new Parameter(100)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }


        [Test]
        public void ValidateJRO_ACC_NoErrors()
        {
            var code = new List<Command> { new Command(Instruction.JRO, 0, new Parameter(Param.ACC)) };

            var errors = Validator.Validate(allLinks, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }
    }
}
