using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Validation
{
    // The Move instruction is the most complex with two parameters
    // that may be any combination of directions, register or number

    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ValidateMovTests
    {
        private readonly HashSet<Direction> allLinks = new HashSet<Direction> { Direction.LEFT, Direction.UP, Direction.RIGHT, Direction.DOWN };

        [Test]
        public void ValidateMov_Parameterless_InvalidTokenErrors()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0) };
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


        [Test]
        public void ValidateMov_FirstInvalidParameter_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(), new Parameter(Direction.UP)) };
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(0, e1.token);
        }

        [Test]
        public void ValidateMov_SecondInvalidParameter_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(Direction.UP), new Parameter()) };
            var links = allLinks;

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        public void ValidateMov_FirstInvalidDirection_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(Direction.UP), new Parameter(Direction.DOWN)) };
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
        public void ValidateMov_SecondInvalidDirection_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(Direction.UP), new Parameter(Direction.DOWN)) };
            var links = new HashSet<Direction> { Direction.UP };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        public void ValidateMov_SecondInvalidNumber_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(Direction.UP), new Parameter(8)) };
            var links = new HashSet<Direction> { Direction.UP };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        public void ValidateMov_ValidNumberAcc_NoErrors()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(8), new Parameter(Param.ACC)) };
            var links = new HashSet<Direction> { };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateMov_ValidNumberNil_NoErrors()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(8), new Parameter(Param.Text, "NIL")) };
            var links = new HashSet<Direction> { };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateMov_ValidNumberDest_NoErrors()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(8), new Parameter(Direction.UP)) };
            var links = new HashSet<Direction> { Direction.UP };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }

        [Test]
        public void ValidateMov_ValidNumberInvalidDest_InvalidTokenError()
        {
            var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(8), new Parameter(Param.Text, "MOV")) };
            var links = new HashSet<Direction> { Direction.UP };

            var errors = Validator.Validate(links, code);

            Assert.IsNotNull(errors);
            Assert.IsNotEmpty(errors);
            Assert.AreEqual(1, errors.Count);

            var e1 = errors[0];
            Assert.AreEqual(0, e1.line);
            Assert.AreEqual(1, e1.token);
        }

        [Test]
        public void ValidateMov_AllValidDestDest_NoErrors()
        {
            var errors = new List<Error>();
            foreach (var firstLink in allLinks)
            {
                foreach (var secondLink in allLinks)
                {
                    var code = new List<Command> { new Command(Instruction.MOV, 0, new Parameter(firstLink), new Parameter(secondLink)) };
                    errors.AddRange(Validator.Validate(allLinks, code));
                }
            }

            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }
    }
}
