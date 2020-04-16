using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public abstract class PqlRelation : PqlAst
    {
        public PqlAst LeftRef;
        public PqlAst RightRef;

        public PqlRelation(PqlAst leftRef, PqlAst rightRef)
        {
            LeftRef = leftRef;
            RightRef = rightRef;
        }
    }
}
