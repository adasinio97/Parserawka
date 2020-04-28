using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class ProcedureUses
    {
        public Procedure Procedure { get; set; }
        public Variable Variable { get; set; }

        public ProcedureUses(Procedure procedure, Variable variable)
        {
            Procedure = procedure;
            Variable = variable;
        }
    }
}
