using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstCall : AST
    {
        public Token ProcedureToken { get; set; }

        public AstCall(Token proc, int line)
        {
            //todo zapisanie linii po dziedziczeniu ze statementa
            ProcedureToken = proc;
        }

        public override string ToString()
        {
            return "CALL" + ProcedureToken.Value;
        }
    }
}
