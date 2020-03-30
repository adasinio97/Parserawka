using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    class AstEquation : AST
    {
        public AST Left { get; set; }
        public AST Right { get; set; }

        public Token Operation { get; set; }

        public AstEquation(AST left, Token op, AST right)
        {
            Left = left;
            Right = right;
            Operation = op;
        }

        public override string ToString()
        {
            return Left.ToString() + Operation.Value.ToString() + Right.ToString();
        }
    }
}
