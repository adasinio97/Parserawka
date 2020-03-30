using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstUnOp : AST
    {
        public Token op { get; set; }
        public AST expr { get; set; }

        public AstUnOp(Token op, AST expr)
        {
            this.op = op;
            this.expr = expr;
        }

        public override string ToString()
        {
            return op.Value.ToString() + expr.ToString();
        }
    }
}
