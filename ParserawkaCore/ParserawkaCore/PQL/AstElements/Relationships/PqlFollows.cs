using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlFollows : PqlRelation
    {		
		public PqlFollows(PqlAst leftRef, PqlAst rightRef) : base(leftRef, rightRef) { }
    }
}
