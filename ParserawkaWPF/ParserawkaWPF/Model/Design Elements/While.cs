using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
using ParserawkaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class While : Statement
    {
        public Variable Condition { get; set; }
        public IStatementList Body { get; set; }

        public While(int programLine) : base(programLine)
        {
            Body = ImplementationFactory.CreateStatementList();
        }

        public While(Variable condition, IStatementList body, int programLine) : base(programLine)
        {
            Condition = condition;
            Body = body;
        }

        public override string ToString()
        {
            return base.ToString() + " while " + Condition.ToString() + " then ";
        }
    }
}
