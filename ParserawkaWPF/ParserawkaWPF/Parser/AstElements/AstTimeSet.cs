using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstTimeSet : AST
    {
        public Token hour, minute, second;

        public AstTimeSet(Token h, Token m, Token s)
        {
            hour = h;
            minute = m;
            second = s;
        }

        public override string ToString()
        {
            return hour.Value.ToString() + ":" + minute.Value.ToString() + ":" + second.Value.ToString();
        }
    }
}
