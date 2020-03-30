using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    class AstAssert : AST
    {
        public AST Condition { get; set; }

        public Token Message { get; set; }

        public AstAssert(AST condition, Token message)
        {
            Condition = condition;
            Message = message;
        }

        public override string ToString()
        {
            return Message.Value.ToString();
        }
    }
}
