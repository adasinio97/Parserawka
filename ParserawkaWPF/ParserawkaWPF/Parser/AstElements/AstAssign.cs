using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstAssign : AstStatement
    {
        public AstVariable Left { get; set; }
        public AST Right { get; set; }

        public Token Operation { get; set; }

        public AstAssign(AstVariable left, Token op, AST right)
        {
            Left = left;
            Right = right;
            Operation = op;
        }

        public override string ToString()
        {
            return Left.name + Operation.Value.ToString() + Right.ToString();
        }
    }
}
