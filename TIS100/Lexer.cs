using System.Collections.Generic;
using TIS100.Types;

/* The Lexer
 * 
 * This is a two stage Lexer which first identifies the tokens in the input stream
 * then uses that information to produce a list of Symbols with the corresponding
 * data.
 * 
 * Tokenizing can be done forward looking only which allows for a simple implementation.
 * 
 * Joining the tokens does perform some backwards searching for Labels as they are defined 
 * at the end of their symbols.
 * 
 */

namespace TIS100
{
    public static class Lexer
    {
        public static List<Symbol> Lex(string input)
        {
            var tokens = Join(Tokenize(input), input);

            tokens.RemoveAll(t => t.token == Token.Whitespace);

            return tokens;
        }

        internal static List<Token> Tokenize(string input)
        {
            if (input == null || input.Length == 0)
            {
                return new List<Token>();
            }

            // Line breaks are replaced with single characters
            // allowing single character tokenization
            input = input.Replace("\r\n", "\n");

            var tokens = new List<Token>();
            
            foreach (char c in input)
            {
                switch (c)
                {
                    case ' ' :
                        tokens.Add(Token.Whitespace);
                        break;
                    case '\n':
                        tokens.Add(Token.Newline);
                        break;
                    case ',':
                        tokens.Add(Token.Comma);
                        break;
                    case '#':
                        tokens.Add(Token.Comment);
                        break;
                    case ':':
                        tokens.Add(Token.Label);
                        break;
                    case '-':
                        tokens.Add(Token.Number);
                        break;
                    // Using the ASCII structure that stores all numbers
                    // between 48 and 57
                    case char n when 48 <= n && n <= 57:
                        tokens.Add(Token.Number);
                        break;
                    default:
                        tokens.Add(Token.Text);
                        break;
                }
            }

            return tokens;
        }

        internal static List<Symbol> Join(IList<Token> tokens, string input)
        {
            // Due to the way testing is structured this line break conversion
            // has to be repeated to keep the input lengths constant
            input = input.Replace("\r\n", "\n");

            var result = new List<Symbol>();
            Token token;
            int tokenCounter = 0;

            while (tokenCounter < tokens.Count)
            {
                token = tokens[tokenCounter];

                switch (token)
                {
                    case Token.Whitespace:
                        while (tokenCounter < tokens.Count && tokens[tokenCounter] == Token.Whitespace) { tokenCounter++; }
                        result.Add(new Symbol(Token.Whitespace, " "));
                        break;
                    case Token.Text:
                        var start = tokenCounter;
                        while (tokenCounter < tokens.Count && tokens[tokenCounter] == Token.Text) { tokenCounter++; }
                        result.Add(new Symbol(Token.Text, input[start..tokenCounter]));
                        break;
                    case Token.Number:
                        var nstart = tokenCounter;
                        while (tokenCounter < tokens.Count && tokens[tokenCounter] == Token.Number) { tokenCounter++; }
                        result.Add(new Symbol(Token.Number, input[nstart..tokenCounter]));
                        break;
                    case Token.Label:
                        // Labels are defined at the end of text, they must backtrack
                        // to convert the previous group from text to a label
                        var prior = result[^1];
                        result[^1] = new Symbol(Token.Label, prior.value);
                        tokenCounter++;
                        break;
                    case Token.Comment:
                        // Comments continue until the end of the line
                        // Double comment for titles are not supported
                        var cstart = tokenCounter++;
                        while (tokenCounter < tokens.Count && tokens[tokenCounter] != Token.Newline) { tokenCounter++; }
                        result.Add(new Symbol(Token.Comment, input[cstart..tokenCounter]));
                        break;
                    case Token.Newline:
                        result.Add(new Symbol(Token.Newline, ""));
                        tokenCounter++;
                        break;
                    default:
                        tokenCounter++;
                        break;
                }
            }

            return result;
        }
    }
}
