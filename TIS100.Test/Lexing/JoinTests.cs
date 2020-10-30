using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class JoinTests
    {
        [Test]
        public void Join_OnlyWhitespace_SingleWhitespace([Random(1, 10, 1)] int number)
        {
            string input = new string(' ', number);
            var tokens = Lexer.Tokenize(input);


            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            
            var first = result[0];
            Assert.AreEqual(Token.Whitespace, first.token);
            Assert.AreEqual(" ", first.value);
        }

        [Test]
        public void Join_OnlyText_SingleText([Random(1, 10, 1)] int number)
        {
            string input = new string('A', number);
            var tokens = Lexer.Tokenize(input);


            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Text, first.token);
            Assert.AreEqual(input, first.value);
        }

        [Test]
        public void Join_TwoText_TwoText([Random(1, 10, 1)] int first, [Random(1, 10, 1)] int second)
        {
            string inputFirst = new string('A', first);
            string inputSecond = new string('B', second);

            string input = inputFirst + " " + inputSecond;

            var tokens = Lexer.Tokenize(input);


            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Token.Text, r0.token);
            Assert.AreEqual(inputFirst, r0.value);

            var r1 = result[1];
            Assert.AreEqual(Token.Whitespace, r1.token);
            Assert.AreEqual(" ", r1.value);

            var r2 = result[2];
            Assert.AreEqual(Token.Text, r2.token);
            Assert.AreEqual(inputSecond, r2.value);
        }

        [Test]
        public void Join_OnlyNumber_SingleNumber()
        {
            const string input = "0123456789";
            var tokens = Lexer.Tokenize(input);


            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Number, first.token);
            Assert.AreEqual(input, first.value);
        }

        [Test]
        public void Join_NegativeNumber_SingleNumber()
        {
            const string input = "-100";
            var tokens = Lexer.Tokenize(input);


            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Number, first.token);
            Assert.AreEqual(input, first.value);
        }

        [Test]
        public void Join_TextLabel_SingleLabel([Random(1, 10, 1)] int number)
        {
            string text = new string('A', number);
            string input = text + ":";

            var tokens = Lexer.Tokenize(input);

            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Label, first.token);
            Assert.AreEqual(text, first.value);
        }


        [Test]
        public void Join_TextLabelText_LabelText([Random(1, 10, 1)] int number)
        {
            string text = new string('A', number);
            string input = text + ": NEG";

            var tokens = Lexer.Tokenize(input);

            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Token.Label, r0.token);
            Assert.AreEqual(text, r0.value);

            var r1 = result[1];
            Assert.AreEqual(Token.Whitespace, r1.token);
            Assert.AreEqual(" ", r1.value);

            var r2 = result[2];
            Assert.AreEqual(Token.Text, r2.token);
            Assert.AreEqual("NEG", r2.value);
        }

        [Test]
        public void Join_CommentText_SingleComment([Random(1, 10, 1)] int number)
        {
            string text = new string('A', number);
            string input = "#" + text;

            var tokens = Lexer.Tokenize(input);
            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Comment, first.token);
            Assert.AreEqual(input, first.value);
        }

        [Test]
        public void Join_CommentMultiText_SingleComment([Random(1, 10, 1)] int number)
        {
            string text = new string('A', number);
            string input = "#" + text + " " + text;

            var tokens = Lexer.Tokenize(input);
            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var first = result[0];
            Assert.AreEqual(Token.Comment, first.token);
            Assert.AreEqual(input, first.value);
        }

        [Test]
        public void Join_TextMultiLine_TwoText([Random(1, 10, 1)] int first, [Random(1, 10, 1)] int second)
        {
            string inputFirst = new string('A', first);
            string inputSecond = new string('B', second);
            string input = inputFirst + @"
" + inputSecond;

            var tokens = Lexer.Tokenize(input);
            var result = Lexer.Join(tokens, input);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Token.Text, r0.token);
            Assert.AreEqual(inputFirst, r0.value);

            var r1 = result[1];
            Assert.AreEqual(Token.Newline, r1.token);

            var r2 = result[2];
            Assert.AreEqual(Token.Text, r2.token);
            Assert.AreEqual(inputSecond, r2.value);
        }
    }
}

