using ParserawkaWPF.Parser.Exceptions;
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
                { "procedure", new Token(TokenType.PROCEDURE, "PROCEDURE") },
                { "call", new Token(TokenType.CALL, "CALL") },

                { "if", new Token(TokenType.IF, "IF") },
                { "then", new Token(TokenType.THEN, "THEN") },
                { "else", new Token(TokenType.ELSE, "ELSE") },
                { "while", new Token(TokenType.WHILE, "WHILE") },
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
            while (currentChar != '\0' && char.IsWhiteSpace(currentChar))
            {
                Advance();
            }
        }

        public Token Number()
        {
            StringBuilder sb = new StringBuilder();

            while (currentChar != '\0' && char.IsDigit(currentChar))
            {
                sb.Append(currentChar);
                Advance();
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

                if (currentChar == '+')
                {
                    Advance();
                    return new Token(TokenType.PLUS, "+");
                }

                if (currentChar == '-')
                {
                    Advance();
                    return new Token(TokenType.MINUS, "-");
                }

                if (currentChar == '*')
                {
                    Advance();
                    return new Token(TokenType.MUL, "*");
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

                if (currentChar == '=')
                {
                    Advance();
                    return new Token(TokenType.ASSIGN, "=");
                }

                throw new LexerException("Nie rozpoznano znaku '" + currentChar + "'", lineCounter, rowCounter);
            }
            return new Token(TokenType.EOF, null);
        }
    }
}
