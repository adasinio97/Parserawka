using ParserawkaCore.Parser;
using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class Assign : Statement
    {
        public Variable Left { get; set; }
        public Factor Right { get; set; }

        public Token Operation { get; set; }

        public Assign(Variable left, Token operation, Factor right, int programLine) : base(programLine)
        {
            Left = left;
            Operation = operation;
            Right = right;
        }

        public override string ToString()
        {
            return base.ToString() + " " + Left.Name + Operation.Value.ToString() + Right.ToString();
        }
    }
}
