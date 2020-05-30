using ParserawkaCore.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class Call : Statement
    {
        public Procedure Procedure { get; set; }
        public string ProcedureName { get; }
        public override Attribute SecondaryAttribute { get; set;}

        public Token ProcedureToken { get; }

        public Call(Procedure procedure, int programLine) : base(programLine)
        {
            Procedure = procedure;
            ProcedureName = procedure.Attribute.AttributeValue;
            SecondaryAttribute = new Attribute("procName", ProcedureName);
        }

        public Call(Token procedureToken, int programLine) : base(programLine)
        {
            ProcedureToken = procedureToken;
            ProcedureName = procedureToken.Value.ToString();
            SecondaryAttribute = new Attribute("procName", ProcedureName);
        }

        public override string ToString()
        {
            return base.ToString() + " Call " + ProcedureName;
        }
    }
}
