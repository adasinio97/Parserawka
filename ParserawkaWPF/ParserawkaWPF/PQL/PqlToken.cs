using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL
{
    public enum PqlTokenType
    {
        //podstawa
        IDENT,
        INTEGER,

        //operatory
        PROCEDURE,
        STMTLST,
        STMT,
        ASSIGN,
        CALL,
        WHILE,
        IF,
        VARIABLE,
        CONSTANT,
        PROG_LINE,

        //atrybuty
        ATTRIBUTE,

        //polecenia
        SELECT,
        SUCH,
        THAT,
        WITH,
        PATTERN,
        AND,

        //relacje
        MODIFIES,
        USES,
        CALLS,
        CALLST,
        PARENT,
        PARENTT,
        FOLLOWS,
        FOLLOWST,
        NEXT,
        NEXTT,
        AFFECTS,
        AFFECTST,

        //znaki
        LPAREN,
        RPAREN,
        LTRIAN,
        RTRIAN,
        COMMA,
        SEMI,
        DOT,
        FLOOR,
        QUOT,
        EQ,
        //dodatkowe
        BOOLEAN,
        EOF,
        
    }
    public class PqlToken
    {
        public PqlTokenType Type { get; set; }
        public object Value { get; set; }

        public PqlToken(PqlTokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return "PqlToken(" + Type.ToString() + "," + Value.ToString() + ")";
        }
    }
}
