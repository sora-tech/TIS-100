using System;
using System.Collections.Generic;
using TIS100.Types;

/* The Parser
 * 
 * Once the input stream has been categorized by the Lexer it can be parsed
 * to produce a sequence of executable commands. This is a naive parser
 * which does not stop if the sequence is malformed.
 * e.g. MOV MOV MOV will be turned into a single MOV command with MOV and MOV
 * as the options.  Validation is done by the Validator class.
 */

namespace TIS100
{
    public static class Parser
    {
        // Covert a given token and value into a typed Parameter
        private static Parameter Convert(Token token, string value)
        {
            if(value == "ACC")
            {
                return new Parameter(Param.ACC);
            }

            if(int.TryParse(value, out var num))
            {
                return new Parameter(num);
            }

            if (Enum.TryParse(value, out Direction dir))
            {
                return new Parameter(dir);
            }

            return token switch
            {
                Token.Label => new Parameter(Param.Label, value),
                Token.Text => new Parameter(Param.Text, value),
                _ => new Parameter(Param.Empty, value),
            };
        }

        public static List<Command> Instruct(IList<Symbol> symbols)
        {
            var result = new List<Command>();

            int symbolCounter = 0;
            int line = 0;
            Symbol symbol;

            while(symbolCounter < symbols.Count)
            {
                symbol = symbols[symbolCounter];

                if (symbol.token == Token.Label)
                {
                    result.Add(new Command(Instruction.LBL, line, new Parameter(Param.Label, symbol.value)));
                    // Check if the label has any code after the definition
                    // LABEL: INSTRUCTION
                    // is a valid label and instruction
                    if(++symbolCounter != symbols.Count && symbols[symbolCounter].token == Token.Newline)
                    {
                        line++;
                    }
                    continue;
                }

                if (symbol.token != Token.Text)
                {
                    if (symbol.token == Token.Comment)
                    {
                        line++;
                    }
                    symbolCounter++;
                    continue;
                }

                // This switch could be refactored to a hash table for easier extension
                switch (symbol.value)
                {
                    case "NOP":
                        result.Add(new Command(Instruction.NOP, line++));
                        symbolCounter++;
                        break;
                    case "MOV":
                        var movp1 = Convert(symbols[++symbolCounter].token, symbols[symbolCounter].value);
                        var movp2 = Convert(symbols[++symbolCounter].token, symbols[symbolCounter].value);
                        result.Add(new Command(Instruction.MOV, line++, movp1, movp2));
                        symbolCounter++;
                        break;
                    case "SWP":
                        result.Add(new Command(Instruction.SWP, line++));
                        symbolCounter++;
                        break;
                    case "SAV":
                        result.Add(new Command(Instruction.SAV, line++));
                        symbolCounter++;
                        break;
                    case "ADD":
                        var addp1 = Convert(symbols[++symbolCounter].token, symbols[symbolCounter].value);
                        result.Add(new Command(Instruction.ADD, line++, addp1));
                        symbolCounter++;
                        break;
                    case "SUB":
                        var subp1 = Convert(symbols[++symbolCounter].token, symbols[symbolCounter].value);
                        result.Add(new Command(Instruction.SUB, line++, subp1));
                        symbolCounter++;
                        break;
                    case "NEG":
                        result.Add(new Command(Instruction.NEG, line++));
                        symbolCounter++;
                        break;
                    case "JMP":
                        var jmpp1 = new Parameter(Param.Text, symbols[++symbolCounter].value);
                        result.Add(new Command(Instruction.JMP, line++, jmpp1));
                        symbolCounter++;
                        break;
                    case "JEZ":
                        var jezp1 = new Parameter(Param.Text, symbols[++symbolCounter].value);
                        result.Add(new Command(Instruction.JEZ, line++, jezp1));
                        symbolCounter++;
                        break;
                    case "JNZ":
                        var jnzp1 = new Parameter(Param.Text, symbols[++symbolCounter].value);
                        result.Add(new Command(Instruction.JNZ, line++, jnzp1));
                        symbolCounter++;
                        break;
                    case "JGZ":
                        var jgzp1 = new Parameter(Param.Text, symbols[++symbolCounter].value);
                        result.Add(new Command(Instruction.JGZ, line++, jgzp1));
                        symbolCounter++;
                        break;
                    case "JLZ":
                        var jlzp1 = new Parameter(Param.Text, symbols[++symbolCounter].value);
                        result.Add(new Command(Instruction.JLZ, line++, jlzp1));
                        symbolCounter++;
                        break;
                    case "JRO":
                        var jrop1 = Convert(symbols[++symbolCounter].token, symbols[symbolCounter].value);
                        result.Add(new Command(Instruction.JRO, line++, jrop1));
                        symbolCounter++;
                        break;
                    default:
                        symbolCounter++;
                        break;
                }
            }
            
            return result;
        }
    }
}
