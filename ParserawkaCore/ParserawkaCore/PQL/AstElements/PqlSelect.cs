using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlSelect : PqlAst
    {
        public List<PqlDeclaration> Declarations;
        public PqlResult Result;
        public List<PqlClause> Clauses;

        public PqlSelect(List<PqlDeclaration> declarations, PqlResult result, List<PqlClause> clauses)
        {
            Declarations = declarations;
            Result = result;
            Clauses = clauses;
        }
    }
}
