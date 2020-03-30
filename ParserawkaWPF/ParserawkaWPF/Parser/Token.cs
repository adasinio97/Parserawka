using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser
{
    public enum TokenType
    {
        PROCEDURE,
        CALL,

        INTEGER,
        ID,
        STRING,

        IF,
        THEN,
        ELSE,
        WHILE,

        ASSIGN,

        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,

        PLUS,
        MINUS,
        MUL,

        SEMI,
        COMMA,
        EOL,
        EOF,
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public object Value { get; set; }

        public Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return "Token(" + Type.ToString() + "," + Value.ToString() + ")";
        }
    }
}
