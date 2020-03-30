using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstWhileStatement : AstStatement
    {
        public AST condition;
        public AST body;

        public AstWhileStatement(AST condition, AST body, int line) : base(line)
        {
            this.condition = condition;
            this.body = body;
        }
        public override string ToString()
        {
            return "WHILE " + condition.ToString() + "{" + body.ToString() + "}";
        }
    }
}
