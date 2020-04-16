using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlAffectsT : PqlRelation
    {
		public PqlAffectsT(PqlAst leftRef, PqlAst rightRef) : base(leftRef, rightRef) { }
    }
}
