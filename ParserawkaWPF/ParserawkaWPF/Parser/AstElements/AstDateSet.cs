using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstDateSet : AST
    {
        public Token day, month, year;

        public AstDateSet(Token d, Token m, Token y)
        {
            day = d;
            month = m;
            year = y;
        }

        public override string ToString()
        {
            return day.Value.ToString() + ":" + month.Value.ToString() + ":" + year.Value.ToString();
        }
    }
}
