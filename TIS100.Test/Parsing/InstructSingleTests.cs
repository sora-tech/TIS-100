using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TIS100.Types;

namespace TIS100.Test.Parsing
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class InstructSingleTests
    {
        // Test each instruction is parsed with the expected number of parameters
        [Test]
        [TestCase(Instruction.NOP, "", "")]
        [TestCase(Instruction.MOV, "A", "B")]
        [TestCase(Instruction.SWP, "", "")]
        [TestCase(Instruction.SAV, "", "")]
        [TestCase(Instruction.ADD, "A", "")]
        [TestCase(Instruction.SUB, "A", "")]
        [TestCase(Instruction.NEG, "A", "")]
        [TestCase(Instruction.JMP, "A", "")]
        [TestCase(Instruction.JEZ, "A", "")]
        [TestCase(Instruction.JNZ, "A", "")]
        [TestCase(Instruction.JGZ, "A", "")]
        [TestCase(Instruction.JLZ, "A", "")]
        [TestCase(Instruction.JRO, "A", "")]
        public void Instruct_Text_Instruction(Instruction ins, string a, string b)
        {
            string text = Enum.GetName(typeof(Instruction), ins);

            var input = Lexer.Lex($"{text} {a} {b}");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(ins, r0.instruction);
        }

        // Comments are ignored
        [Test]
        public void Instruct_Comment_Ignores()
        {
            var input = new Symbol(Token.Comment, "#test");

            var result = Parser.Instruct(new List<Symbol> { input });

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }


        [Test]
        public void Instruct_MOVString_HasSourceDestination()
        {
            var input = Lexer.Lex("MOV UP ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);


            var r0 = result[0];
            Assert.AreEqual(Instruction.MOV, r0.instruction);
            Assert.AreEqual(Direction.UP, r0.primary.direction);
            Assert.AreEqual(Param.Direction, r0.primary.param);

            Assert.AreEqual(Param.ACC, r0.secondary.param);
            Assert.AreEqual("", r0.secondary.value);
        }

        [Test]
        public void Instruct_MOVNumber_HasSourceDestination()
        {
            var input = Lexer.Lex("MOV 10 ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);


            var r0 = result[0];
            Assert.AreEqual(Instruction.MOV, r0.instruction);
            
            Assert.AreEqual(Param.Number, r0.primary.param);
            Assert.AreEqual("", r0.primary.value);
            Assert.AreEqual(10, r0.primary.number);
            

            Assert.AreEqual(Param.ACC, r0.secondary.param);
        }

        // Commas are optional and must be ignored if present
        [Test]
        public void Instruct_MOVCommaNumber_HasSourceDestination()
        {
            var input = Lexer.Lex("MOV 10, ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.MOV, r0.instruction);
            Assert.AreEqual("", r0.primary.value);
            Assert.AreEqual(10, r0.primary.number);
            Assert.AreEqual(Param.Number, r0.primary.param);

            Assert.AreEqual(Param.ACC, r0.secondary.param);
        }

        [Test]
        public void Instruct_ADDNumber_HasSource()
        {
            var input = Lexer.Lex("ADD 10");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.ADD, r0.instruction);
            Assert.AreEqual("", r0.primary.value);
            Assert.AreEqual(10, r0.primary.number);
            Assert.AreEqual(Param.Number, r0.primary.param);
        }

        [Test]
        public void Instruct_ADDACC_HasSource()
        {
            var input = Lexer.Lex("ADD ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.ADD, r0.instruction);
            Assert.AreEqual(Param.ACC, r0.primary.param);
        }

        [Test]
        public void Instruct_ADDDirection_HasSource()
        {
            var input = Lexer.Lex("ADD UP");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.ADD, r0.instruction);
            Assert.AreEqual(Direction.UP, r0.primary.direction);
            Assert.AreEqual(Param.Direction, r0.primary.param);
        }

        [Test]
        public void Instruct_SUBNumber_HasSource()
        {
            var input = Lexer.Lex("SUB 10");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.SUB, r0.instruction);
            
            Assert.AreEqual("", r0.primary.value);
            Assert.AreEqual(10, r0.primary.number);
            Assert.AreEqual(Param.Number, r0.primary.param);
        }

        [Test]
        public void Instruct_SUBACC_HasSource()
        {
            var input = Lexer.Lex("SUB ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.SUB, r0.instruction);
            Assert.AreEqual(Param.ACC, r0.primary.param);
        }

        [Test]
        public void Instruct_SUBDirection_HasSource()
        {
            var input = Lexer.Lex("SUB UP");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.SUB, r0.instruction);
            Assert.AreEqual(Direction.UP, r0.primary.direction);
            Assert.AreEqual(Param.Direction, r0.primary.param);
        }


        [Test]
        public void Instruct_JMPText_HasSource()
        {
            var input = Lexer.Lex("JMP START");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JMP, r0.instruction);
            Assert.AreEqual("START", r0.primary.value);
        }

        [Test]
        public void Instruct_JEZText_HasSource()
        {
            var input = Lexer.Lex("JEZ START");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JEZ, r0.instruction);
            Assert.AreEqual("START", r0.primary.value);
        }

        [Test]
        public void Instruct_JGZText_HasSource()
        {
            var input = Lexer.Lex("JGZ START");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JGZ, r0.instruction);
            Assert.AreEqual("START", r0.primary.value);
        }

        [Test]
        public void Instruct_JLZText_HasSource()
        {
            var input = Lexer.Lex("JLZ START");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JLZ, r0.instruction);
            Assert.AreEqual("START", r0.primary.value);
        }

        [Test]
        public void Instruct_JRONumber_HasSource()
        {
            var input = Lexer.Lex("JRO 5");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JRO, r0.instruction);
            Assert.AreEqual("", r0.primary.value);
            Assert.AreEqual(5, r0.primary.number);
            Assert.AreEqual(Param.Number, r0.primary.param);
        }


        [Test]
        public void Instruct_JRODirection_HasSource()
        {
            var input = Lexer.Lex("JRO UP");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JRO, r0.instruction);
            Assert.AreEqual(Direction.UP, r0.primary.direction);
            Assert.AreEqual(Param.Direction, r0.primary.param);
        }


        [Test]
        public void Instruct_JROACC_HasSource()
        {
            var input = Lexer.Lex("JRO ACC");

            var result = Parser.Instruct(input);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var r0 = result[0];
            Assert.AreEqual(Instruction.JRO, r0.instruction);

            Assert.AreEqual(Param.ACC, r0.primary.param);
        }
    }
}

