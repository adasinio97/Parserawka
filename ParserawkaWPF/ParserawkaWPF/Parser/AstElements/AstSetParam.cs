using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstSetParam : AST
    {
        public AstVariable id;
        public Token operation;
        public AST expr;

        public AstSetParam(AstVariable id, Token operation, AST expr)
        {
            this.id = id;
            this.operation = operation;
            this.expr = expr;
        }

        public override string ToString()
        {
            return id.name + operation.Value.ToString() + expr.ToString();
        }
    }
}
