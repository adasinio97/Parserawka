using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL
{
    public class PqlLexer
    {
        public string Tekst { get; set; }

        public int lineCounter;
        public int rowCounter;

        public Dictionary<string, PqlToken> ReservedIDs { get; set; }
        private Dictionary<string, PqlToken> InitIDs = new Dictionary<string, PqlToken>
        {
                { "procedure", new PqlToken(PqlTokenType.PROCEDURE, "PROCEDURE") },
                { "stmtlst", new PqlToken(PqlTokenType.STMTLST, "STMTLST") },
                { "stmt", new PqlToken(PqlTokenType.STMT, "STMT") },
                { "assign", new PqlToken(PqlTokenType.ASSIGN, "ASSIGN") },
                { "call", new PqlToken(PqlTokenType.CALL, "CALL") },
                { "while", new PqlToken(PqlTokenType.WHILE, "WHILE") },
                { "if", new PqlToken(PqlTokenType.IF, "IF") },
                { "variable", new PqlToken(PqlTokenType.VARIABLE, "VARIABLE") },
                { "constant", new PqlToken(PqlTokenType.CONSTANT, "CONSTANT") },
                { "prog_line", new PqlToken(PqlTokenType.PROG_LINE, "PROG_LINE") },

                { "procName", new PqlToken(PqlTokenType.ATTRIBUTE, "procName") },
                { "varName", new PqlToken(PqlTokenType.ATTRIBUTE, "varName") },
                { "value", new PqlToken(PqlTokenType.ATTRIBUTE, "value") },
                { "stmt#", new PqlToken(PqlTokenType.ATTRIBUTE, "stmt") },

                { "Select", new PqlToken(PqlTokenType.SELECT, "SELECT") },
                { "such", new PqlToken(PqlTokenType.SUCH, "SUCH") },
                { "that", new PqlToken(PqlTokenType.THAT, "THAT") },
                { "with", new PqlToken(PqlTokenType.WITH, "WITH") },
                { "pattern", new PqlToken(PqlTokenType.PATTERN, "PATTERN") },
                { "and", new PqlToken(PqlTokenType.AND, "AND") },

                { "Modifies", new PqlToken(PqlTokenType.MODIFIES, "MODIFIES") },
                { "Uses", new PqlToken(PqlTokenType.USES, "USES") },
                { "Calls", new PqlToken(PqlTokenType.CALLS, "CALLS") },
                { "Calls*", new PqlToken(PqlTokenType.CALLST, "CALLST") },
                { "Parent", new PqlToken(PqlTokenType.PARENT, "PARENT") },
                { "Parent*", new PqlToken(PqlTokenType.PARENTT, "PARENTT") },
                { "Follows", new PqlToken(PqlTokenType.FOLLOWS, "FOLLOWS") },
                { "Follows*", new PqlToken(PqlTokenType.FOLLOWST, "FOLLOWST") },
                { "Next", new PqlToken(PqlTokenType.NEXT, "NEXT") },
                { "Next*", new PqlToken(PqlTokenType.NEXTT, "NEXTT") },
                { "Affects", new PqlToken(PqlTokenType.AFFECTS, "AFFECTS") },
                { "Affects*", new PqlToken(PqlTokenType.AFFECTST, "AFFECTST") },

                { "BOOLEAN", new PqlToken(PqlTokenType.BOOLEAN, "BOOLEAN") },
        };

        private char currentChar;
        private int pos;

        public PqlLexer(string text)
        {
            Tekst = text;
            pos = 0;
            lineCounter = 1;
            rowCounter = 1;
            currentChar = Tekst[0];

            ReservedIDs = new Dictionary<string, PqlToken>(InitIDs);
        }

        public void Reset()
        {
            pos = 0;
            lineCounter = 1;
            rowCounter = 1;
            currentChar = Tekst[0];

            ReservedIDs = new Dictionary<string, PqlToken>(InitIDs);
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
            while (currentChar != '\0' && char.IsWhiteSpace(currentChar))
            {
                Advance();
            }
        }

        public PqlToken Number()
        {
            StringBuilder sb = new StringBuilder();

            while (currentChar != '\0' && char.IsDigit(currentChar))
            {
                sb.Append(currentChar);
                Advance();
            }

            return new PqlToken(PqlTokenType.INTEGER, int.Parse(sb.ToString()));
        }

        public PqlToken Ident()
        {
            StringBuilder sb = new StringBuilder();
            while (currentChar != '\0' && (char.IsLetterOrDigit(currentChar) || currentChar == '#' || currentChar == '*' || currentChar == '_'))
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
                ReservedIDs.Add(sb.ToString(), new PqlToken(PqlTokenType.IDENT, sb.ToString()));
                return ReservedIDs[sb.ToString()];
            }
        }

        public PqlToken String()
        {
            StringBuilder sb = new StringBuilder();
            while (currentChar != '\0' && currentChar != '"')
            {
                sb.Append(currentChar);
                Advance();
            }
            Advance();

            return new PqlToken(PqlTokenType.STRING, sb.ToString());
        }

        public char Peek()
        {
            int peekPos = pos + 1;

            if (peekPos >= Tekst.Length)
                return '\0';
            else
                return Tekst[peekPos];
        }

        public PqlToken GetNextToken()
        {
            while (currentChar != '\0')
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    SkipWhiteSpace();
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    return Number();
                }

                if (char.IsLetter(currentChar))
                {
                    return Ident();
                }

                if (currentChar == '"')
                {
                    return String();
                }

                if (currentChar == '(')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.LPAREN, "(");
                }

                if (currentChar == ')')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.RPAREN, ")");
                }

                if (currentChar == '<')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.LTRIAN, "<");
                }

                if (currentChar == '>')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.RTRIAN, ">");
                }

                if (currentChar == ';')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.SEMI, ";");
                }

                if (currentChar == ',')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.COMMA, ",");
                }

                if (currentChar == '.')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.DOT, ".");
                }

                if (currentChar == '_')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.FLOOR, "_");
                }

                if (currentChar == '=')
                {
                    Advance();
                    return new PqlToken(PqlTokenType.EQ, "=");
                }

                //throw new PqlLexerException("Nie rozpoznano znaku '" + currentChar + "'", lineCounter, rowCounter);
            }
            return new PqlToken(PqlTokenType.EOF, null);
        }

    }
}
