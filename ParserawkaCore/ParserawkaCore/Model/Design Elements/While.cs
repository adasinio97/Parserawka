using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class While : Container
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
