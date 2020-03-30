using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser
{
    public class Lexer
    {
        public string Tekst { get; set; }

        public int lineCounter;
        public int rowCounter;

        public Dictionary<string, Token> ReservedIDs { get; set; }
        private Dictionary<string, Token> InitIDs = new Dictionary<string, Token>
        {
                            { "TEST", new Token(TokenType.TEST, "TEST") },
                { "test", new Token(TokenType.TEST, "TEST") },

                { "time", new Token(TokenType.TIMESET, "TIMESET") },
                { "date", new Token(TokenType.DATESET, "DATESET") },

                { "czas", new Token(TokenType.TIMESET, "TIMESET") },
                { "data", new Token(TokenType.DATESET, "DATESET") },

                { "SET", new Token(TokenType.SET, "SET") },
                { "ASSERT", new Token(TokenType.ASSERT, "ASSERT") },
                { "IMPORT", new Token(TokenType.IMPORT, "IMPORT") },
                { "WAIT", new Token(TokenType.WAIT, "WAIT") },

                { "set", new Token(TokenType.SET, "SET") },
                { "assert", new Token(TokenType.ASSERT, "ASSERT") },
                { "import", new Token(TokenType.IMPORT, "IMPORT") },
                { "wait", new Token(TokenType.WAIT, "WAIT") },

                { "ustaw", new Token(TokenType.SET, "SET") },
                { "sprawdz", new Token(TokenType.ASSERT, "ASSERT") },
                { "importuj", new Token(TokenType.IMPORT, "IMPORT") },
                { "czekaj", new Token(TokenType.WAIT, "WAIT") },

                //{ "IF", new Token(TokenType.IF, "IF") },
                //{ "ELSE", new Token(TokenType.ELSE, "ELSE") },
                //{ "WHILE", new Token(TokenType.WHILE, "WHILE") },

                //{ "if", new Token(TokenType.IF, "IF") },
                //{ "else", new Token(TokenType.ELSE, "ELSE") },
                //{ "while", new Token(TokenType.WHILE, "WHILE") },

                { "DPPARAM", new Token(TokenType.TYPE, "DPPARAM") },
                { "DPPARAMREP", new Token(TokenType.TYPE, "DPPARAMREP") },
                { "BIT", new Token(TokenType.TYPE, "BIT") },
                { "NUMBER", new Token(TokenType.TYPE, "NUMBER") },

                { "dpParam", new Token(TokenType.TYPE, "DPPARAM") },
                { "dpParamRep", new Token(TokenType.TYPE, "DPPARAMREP") },
                { "bit", new Token(TokenType.TYPE, "BIT") },
                { "number", new Token(TokenType.TYPE, "NUMBER") },
        };

        private char currentChar;
        private int pos;

        public Lexer(string text)
        {
            Tekst = text;
            pos = 0;
            lineCounter = 1;
            rowCounter = 1;
            currentChar = Tekst[0];

            ReservedIDs = new Dictionary<string, Token>(InitIDs);
        }

        public void Reset()
        {
            pos = 0;
            lineCounter = 1;
            rowCounter = 1;
            currentChar = Tekst[0];

            ReservedIDs = new Dictionary<string, Token>(InitIDs);
        }

        public void Advance()
        {
            pos++;
            rowCounter++;
            if (currentChar == '\n')
            {
                rowCounter = 1;
                lineCounter++;
            }

            if (pos >= Tekst.Length)
                currentChar = '\0';
            else
                currentChar = Tekst[pos];
        }

        public void SkipWhiteSpace()
        {
            while (currentChar != '\0' && char.IsWhiteSpace(currentChar) && currentChar != '\n')
            {
                Advance();
            }
        }

        public void SkipOneLineComment()
        {
            while (currentChar != '\0' && currentChar != '\n')
            {
                Advance();
            }
        }

        public void SkipMultiLineComment()
        {
            while (currentChar != '\0')
            {
                if (currentChar == '*')
                {
                    Advance();
                    if (currentChar == '/')
                    {
                        Advance();
                        break;
                    }
                }
                else
                {
                    Advance();
                }
            }
        }

        public Token Number()
        {
            StringBuilder sb = new StringBuilder();
            if (currentChar == '0' && Peek() == 'x') //liczba szesnastkowa np. 0x123abc
            {
                while (currentChar != '\0' && char.IsLetterOrDigit(currentChar))
                {
                    sb.Append(currentChar);
                    Advance();
                }
                return new Token(TokenType.BIT, Convert.ToUInt64(sb.ToString(), 16));
            }
            while (currentChar != '\0' && char.IsDigit(currentChar))
            {
                sb.Append(currentChar);
                Advance();
            }
            if (currentChar == '.')
            {
                sb.Append(',');
                Advance();
                while (currentChar != '\0' && char.IsDigit(currentChar))
                {
                    sb.Append(currentChar);
                    Advance();
                }
                return new Token(TokenType.FLOAT, float.Parse(sb.ToString()));
            }
            else if (currentChar == 's' || currentChar == 'm' || currentChar == 'h')
            {
                int multiplier = 1;
                if (currentChar == 's')
                {
                    multiplier = 1;
                }
                if (currentChar == 'h')
                {
                    multiplier = 3600;
                }
                if (currentChar == 'm')
                {
                    multiplier = 60;
                    Advance();
                    Advance();
                }
                Advance();
                return new Token(TokenType.TIME, int.Parse(sb.ToString()) * multiplier);
            }

            return new Token(TokenType.INTEGER, int.Parse(sb.ToString()));
        }

        public Token Id()
        {
            StringBuilder sb = new StringBuilder();
            while (currentChar != '\0' && (char.IsLetterOrDigit(currentChar) || currentChar == '_'))
            {
                sb.Append(currentChar);
                Advance();
            }

            if (ReservedIDs.ContainsKey(sb.ToString()))
            {
                return ReservedIDs[sb.ToString()];
            }
            else
            {
                ReservedIDs.Add(sb.ToString(), new Token(TokenType.ID, sb.ToString()));
                return ReservedIDs[sb.ToString()];
            }
        }

        public Token String()
        {
            StringBuilder sb = new StringBuilder();
            while (currentChar != '\0' && currentChar != '"')
            {
                sb.Append(currentChar);
                Advance();
            }
            Advance();

            return new Token(TokenType.STRING, sb.ToString());
        }

        public char Peek()
        {
            int peekPos = pos + 1;

            if (peekPos >= Tekst.Length)
                return '\0';
            else
                return Tekst[peekPos];
        }

        public Token GetNextToken()
        {
            while (currentChar != '\0')
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    if (currentChar == '\n')
                    {
                        Advance();
                        return new Token(TokenType.EOL, "EOL");
                    }
                    SkipWhiteSpace();
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    return Number();
                }

                if (char.IsLetter(currentChar) || currentChar == '_')
                {
                    return Id();
                }

                if (currentChar == '"')
                {
                    Advance();
                    return String();
                }

                if (currentChar == '.')
                {
                    Advance();
                    return new Token(TokenType.DOT, ".");
                }

                if (currentChar == ',')
                {
                    Advance();
                    return new Token(TokenType.COMMA, ",");
                }

                if (currentChar == '+')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.PLUSEQ, "+=");
                    }
                    return new Token(TokenType.PLUS, "+");
                }

                if (currentChar == '-')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.MINUSEQ, "-=");
                    }
                    return new Token(TokenType.MINUS, "-");
                }

                if (currentChar == '*')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.MULEQ, "*=");
                    }
                    return new Token(TokenType.MUL, "*");
                }

                if (currentChar == '/')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.DIVEQ, "/=");
                    }
                    else if (currentChar == '/')
                    {
                        Advance();
                        SkipOneLineComment();
                        continue;
                    }
                    else if (currentChar == '*')
                    {
                        Advance();
                        SkipMultiLineComment();
                        continue;
                    }
                    return new Token(TokenType.DIV, "/");
                }

                if (currentChar == '(')
                {
                    Advance();
                    return new Token(TokenType.LPAREN, "(");
                }

                if (currentChar == ')')
                {
                    Advance();
                    return new Token(TokenType.RPAREN, ")");
                }

                if (currentChar == '{')
                {
                    Advance();
                    return new Token(TokenType.LBRACE, "{");
                }

                if (currentChar == '}')
                {
                    Advance();
                    return new Token(TokenType.RBRACE, "}");
                }

                if (currentChar == ';')
                {
                    Advance();
                    return new Token(TokenType.SEMI, ";");
                }

                if (currentChar == ':')
                {
                    Advance();
                    return new Token(TokenType.COLON, ":");
                }

                if (currentChar == '=')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.EQ, "==");
                    }
                    return new Token(TokenType.ASSIGN, "=");
                }
                if (currentChar == '<')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.LTEQ, "<=");
                    }
                    return new Token(TokenType.LT, "<");
                }
                if (currentChar == '>')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.GTEQ, ">=");
                    }
                    return new Token(TokenType.GT, "");
                }

                if (currentChar == '!')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.NEQ, "!=");
                    }
                    return new Token(TokenType.NEG, "!");
                }

                if (currentChar == '|')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.BOREQ, "|=");
                    }
                    if (currentChar == '|')
                    {
                        Advance();
                        return new Token(TokenType.OR, "||");
                    }
                    return new Token(TokenType.BOR, "|");
                }

                if (currentChar == '&')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.BANDEQ, "&=");
                    }
                    if (currentChar == '&')
                    {
                        Advance();
                        return new Token(TokenType.AND, "&&");
                    }
                    return new Token(TokenType.BAND, "&");
                }

                if (currentChar == '^')
                {
                    Advance();
                    if (currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.BXOREQ, "^=");
                    }
                    return new Token(TokenType.BXOR, "^");
                }

                if (currentChar == '~')
                {
                    Advance();
                    return new Token(TokenType.BNOT, "~");
                }

                //throw new LexerException("Nie rozpoznano znaku '" + currentChar + "'", lineCounter, rowCounter);
            }
            return new Token(TokenType.EOF, null);
        }
    }
}
