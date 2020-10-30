using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TokenizeMultilineInputTests
    {

        [Test]
        public void Tokenize_InputFullLine_OutputValid()
        {
            const string input = @"JMP:
NEG";
            List<Token> expected = new List<Token> { 
                Token.Text, Token.Text, Token.Text, Token.Label, 
                Token.Newline, Token.Text, Token.Text, Token.Text };

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Tokenize_InputMultiLine_OutputValid()
        {
            const string input = @"MOV LEFT, ACC
ADD ACC";
            List<Token> expected = new List<Token> {
                Token.Text, Token.Text, Token.Text, Token.Whitespace,
                Token.Text, Token.Text, Token.Text, Token.Text, Token.Comma, Token.Whitespace,
                Token.Text, Token.Text, Token.Text, Token.Newline,
                Token.Text, Token.Text, Token.Text, Token.Whitespace,
                Token.Text, Token.Text, Token.Text
            };

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected, result);
        }
    }
}