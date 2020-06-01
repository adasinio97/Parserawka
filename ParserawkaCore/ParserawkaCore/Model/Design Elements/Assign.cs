using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;
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

        public IStatementList Affecting { get; set; }
        public IStatementList AffectedBy { get; set; }

        public Assign(Variable left, Token operation, Factor right, int programLine) : base(programLine)
        {
            Left = left;
            Operation = operation;
            Right = right;
            AffectedBy = ImplementationFactory.CreateStatementList();
            Affecting = ImplementationFactory.CreateStatementList();
        }

        public override string ToString()
        {
            return base.ToString() + " " + Left.ToString() + " = " + Right.ToString();
        }
    }
}
