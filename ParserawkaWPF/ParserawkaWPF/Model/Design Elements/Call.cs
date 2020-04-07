using ParserawkaWPF.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class Call : Statement
    {
        public Procedure Procedure { get; set; }
        public string ProcedureName { get; }

        public Token ProcedureToken { get; }

        public Call(Procedure procedure, int programLine) : base(programLine)
        {
            Procedure = procedure;
        }

        public Call(Token procedureToken, int programLine) : base(programLine)
        {
            ProcedureToken = procedureToken;
            ProcedureName = procedureToken.Value.ToString();
        }

        public override string ToString()
        {
            return base.ToString() + " Call " + ProcedureName;
        }
    }
}
