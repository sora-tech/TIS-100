using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Parsing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class InstructMultiTests
    {
        [Test]
        public void Instruct_TwoLine_TwoInstructions()
        {
            var input = Lexer.Lex(@"MOV UP RIGHT
MOV LEFT DOWN");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(0, result[0].line);
            Assert.AreEqual(1, result[1].line);
        }

        // Comments are ignored but count towards line numbers
        [Test]
        public void Instruct_CommentLine_OneInstructions()
        {
            var input = Lexer.Lex(@"#comment
MOV LEFT DOWN");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(1, result[0].line);
            Assert.AreEqual(Instruction.MOV, result[0].instruction);
        }

        // Labels can be on their own line
        [Test]
        public void Instruct_LabelLine_OneInstructions()
        {
            var input = Lexer.Lex(@"label:
MOV LEFT DOWN");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(0, result[0].line);
            Assert.AreEqual(Instruction.LBL, result[0].instruction);
            Assert.AreEqual("label", result[0].primary.value);

            Assert.AreEqual(1, result[1].line);
            Assert.AreEqual(Instruction.MOV, result[1].instruction);
        }

        // Labels can also be on the same line as instructions
        [Test]
        public void Instruct_LabelLine_TwoInstructions()
        {
            var input = Lexer.Lex(@"A: MOV SUB NEG");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(0, result[0].line);
            Assert.AreEqual(Instruction.LBL, result[0].instruction);
            Assert.AreEqual("A", result[0].primary.value);

            Assert.AreEqual(0, result[1].line);
            Assert.AreEqual(Instruction.MOV, result[1].instruction);
        }


        [Test]
        public void Instruct_LabelNewLine_TwoInstruction()
        {
            var input = Lexer.Lex(@" JMP A
A:");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(0, result[0].line);
            Assert.AreEqual(Instruction.JMP, result[0].instruction);
            Assert.AreEqual("A", result[0].primary.value);

            Assert.AreEqual(1, result[1].line);
            Assert.AreEqual(Instruction.LBL, result[1].instruction);
            Assert.AreEqual("A", result[1].primary.value);
        }
    }
}
