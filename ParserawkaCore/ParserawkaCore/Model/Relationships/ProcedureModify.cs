using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class ProcedureModify
    {
        public Procedure Procedure { get; set; }
        public Variable Variable { get; set; }

        public ProcedureModify(Procedure procedure, Variable variable)
        {
            Procedure = procedure;
            Variable = variable;
        }
    }
}
