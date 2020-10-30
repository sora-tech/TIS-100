using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Lexing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SymbolTests
    {
        [Test]
        public void Lexer_SampleInput_Parses()
        {
            const string input = @"MOV LEFT, ACC
ADD ACC
MOV ACC, RIGHT";

            var result = Lexer.Lex(input);

            Assert.IsNotNull(result);

            Assert.AreEqual(10, result.Count);
        }

        [Test]
        public void Lexer_WhitespaceInput_Removes()
        {
            const string input = @"   ";

            var result = Lexer.Lex(input);

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }
    }
}
