using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser
{
    public enum TokenType
    {
        TEST,

        TIMESET,
        DATESET,

        INTEGER,
        ID,
        FLOAT,
        BIT,
        STRING,
        TIME,
        TYPE,

        SET,
        ASSERT,
        IMPORT,
        WAIT,

        //IF,
        //ELSE,
        //WHILE,

        DOT,
        ASSIGN,

        LPAREN,
        RPAREN,
        LBRACE,
        RBRACE,

        EQ,
        NEQ,
        NEG,
        LT,
        GT,
        LTEQ,
        GTEQ,

        PLUS,
        MINUS,
        MUL,
        DIV,
        AND,
        OR,
        XOR,

        PLUSEQ,
        MINUSEQ,
        MULEQ,
        DIVEQ,

        BNOT,
        BOR,
        BAND,
        BXOR,
        BANDEQ,
        BOREQ,
        BXOREQ,

        SEMI,
        COLON,
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
