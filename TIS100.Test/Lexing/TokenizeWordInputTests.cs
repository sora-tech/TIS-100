using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TokenizeWordInputTests
    {
        [Test]
        public void Tokenize_InputWord_OutputText()
        {
            const string input = "JMP";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);

            Assert.AreEqual(Token.Text, result[0]);
            Assert.AreEqual(Token.Text, result[1]);
            Assert.AreEqual(Token.Text, result[2]);
        }

        [Test]
        public void Tokenize_InputWordComma_OutputText()
        {
            const string input = "JMP,";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(4, result.Count);

            Assert.AreEqual(Token.Text, result[0]);
            Assert.AreEqual(Token.Text, result[1]);
            Assert.AreEqual(Token.Text, result[2]);
            Assert.AreEqual(Token.Comma, result[3]);
        }

        [Test]
        public void Tokenize_InputWordColon_OutputTextLabel()
        {
            const string input = "JMP:";

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(4, result.Count);

            Assert.AreEqual(Token.Text, result[0]);
            Assert.AreEqual(Token.Text, result[1]);
            Assert.AreEqual(Token.Text, result[2]);
            Assert.AreEqual(Token.Label, result[3]);
        }

        [Test]
        public void Tokenize_InputFullLine_OutputValid()
        {
            const string input = "JMP: NEG";
            List<Token> expected = new List<Token> {
                Token.Text, Token.Text, Token.Text, Token.Label,
                Token.Whitespace, Token.Text, Token.Text, Token.Text };

            var result = Lexer.Tokenize(input);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);

            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected, result);
        }
    }
}