using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlPatternCond : PqlClause
    {
        public List<PqlPatternNode> PatternNode;

        public PqlPatternCond(List<PqlPatternNode> patternCond)
        {
            PatternNode = patternCond;
        }
    }
}
