using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlSelect : PqlAst
    {
        public List<PqlAst> Declarations;
        public PqlAst Result;
        public List<PqlAst> Clauses;

        public PqlSelect(List<PqlAst> declarations, PqlAst result, List<PqlAst> clauses)
        {
            Declarations = declarations;
            Result = result;
            Clauses = clauses;
        }
    }
}
