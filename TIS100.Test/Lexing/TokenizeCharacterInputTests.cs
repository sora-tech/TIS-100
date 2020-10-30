using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TokenizeCharacterInputTests
    {
        [Test]
        public void Tokenize_InputEmpty_OutputEmpty()
        {
            const string input = "";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Tokenize_InputNull_OutputEmpty()
        {
            const string input = null;

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Tokenize_InputSingleWhitespace_OutputSingleWhitespace()
        {
            const string input = " ";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Whitespace, result[0]);
        }
        [Test]
        public void Tokenize_InputNWhitespace_OutputNWhitespace([Random(1, 10, 1)]int number)
        {
            string input = new String(' ', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Whitespace, result[0]);
        }

        [Test]
        public void Tokenize_InputSingleNewline_OutputSingleNewline()
        {
            const string input = @"
";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Newline, result[0]);
        }

        [Test]
        public void Tokenize_InputNNewline_OutputNNewLine([Random(1, 10, 1)] int number)
        {
            string input = new String('\n', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Newline, result[0]);
        }

        [Test]
        public void Tokenize_InputSingleHash_OutputComment()
        {
            const string input = "#";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Comment, result[0]);
        }

        [Test]
        public void Tokenize_InputNHash_OutputNComment([Random(1, 10, 1)] int number)
        {
            string input = new String('#', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Comment, result[0]);
        }

        [Test]
        public void Tokenize_InputSingleColon_OutputSingleLabel()
        {
            const string input = ":";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Label, result[0]);
        }

        [Test]
        public void Tokenize_InputNColon_OutputNLabel([Random(1, 10, 1)] int number)
        {
            string input = new String(':', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Label, result[0]);
        }

        [Test]
        public void Tokenize_InputSingleComma_OutputSingleComma()
        {
            const string input = ",";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Comma, result[0]);
        }

        [Test]
        public void Tokenize_InputNComma_OutputNComma([Random(1, 10, 1)] int number)
        {
            string input = new String(',', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Comma, result[0]);
        }

        [Test]
        public void Tokenize_InputSingleCharacter_OutputSingleText()
        {
            const string input = "A";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Text, result[0]);
        }

        [Test]
        public void Tokenize_InputNCharacter_OutputNText([Random(1, 10, 1)] int number)
        {
            string input = new String('A', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Text, result[0]);
        }
    }
}