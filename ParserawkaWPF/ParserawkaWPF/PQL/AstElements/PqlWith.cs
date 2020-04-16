using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlWith : PqlClause
    {
        public List<PqlCompare> AttrCond { get; set; }

        public PqlWith(List<PqlCompare> attrCond)
        {
            AttrCond = attrCond;
        }
    }
}
