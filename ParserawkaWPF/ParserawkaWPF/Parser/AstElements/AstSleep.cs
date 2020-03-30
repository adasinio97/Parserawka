using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    class AstSleep : AST
    {
        public AST expr;

        public AstSleep(AST expr)
        {
            this.expr = expr;
        }
    }
}
