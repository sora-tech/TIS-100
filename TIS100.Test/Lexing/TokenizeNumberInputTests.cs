using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TokenizeNumberInputTests
    {

        [Test]
        public void Tokenize_InputSingleWhitespace_OutputSingleWhitespace()
        {
            const string input = "0";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Token.Number, result[0]);
        }

        [Test]
        public void Tokenize_InputAllWhitespace_OutputSingleWhitespace()
        {
            const string input = "0123456789";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(10, result.Count);

            Assert.AreEqual(Token.Number, result[0]);
        }

        [Test]
        public void Tokenize_InputNWhitespace_OutputNWhitespace([Random(1, 10, 1)]int number)
        {
            string input = new String('1', number);

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(number, result.Count);

            Assert.AreEqual(Token.Number, result[0]);
        }

        [Test]
        public void Tokenize_InputNegative_OutputNumber()
        {
            string input = "-10";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(Token.Number, result[0]);
            Assert.AreEqual(Token.Number, result[1]);
            Assert.AreEqual(Token.Number, result[2]);
        }
    }
}