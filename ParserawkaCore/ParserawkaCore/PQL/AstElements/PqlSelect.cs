using ParserawkaCore.PQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlSelect : PqlAst
    {
        public IDeclarationList Declarations;
        public PqlResult Result;
        public List<PqlWith> WithClauses;
        public List<PqlSuchThat> SuchThatClauses;
        public List<PqlPatternCond> PatternClauses;

        public PqlSelect(IDeclarationList declarations, PqlResult result, List<PqlWith> withClauses, List<PqlSuchThat> suchThatClauses, List<PqlPatternCond> patternClauses) 
        {
            Declarations = declarations;
            Result = result;
            WithClauses = withClauses;
            SuchThatClauses = suchThatClauses;
            PatternClauses = patternClauses;
        }
    }
}
