using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstIfStatement : AST
    {
        public AST condition;
        public AST body, elseBody;

        public AstIfStatement(AST condition, AST body, AST elseBody)
        {
            this.condition = condition;
            this.body = body;
            this.elseBody = elseBody;
        }
    }
}
