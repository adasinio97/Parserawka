using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlSuchThat : PqlClause
    {
        public List<PqlRelation> RelCond { get; set; }

        public PqlSuchThat(List<PqlRelation> relCond)
        {
            RelCond = relCond;
        }
    }
}
