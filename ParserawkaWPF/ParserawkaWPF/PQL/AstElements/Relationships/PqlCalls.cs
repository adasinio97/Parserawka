using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlCalls : PqlRelation
    {		
		public PqlCalls(PqlAst leftRef, PqlAst rightRef) : base(leftRef, rightRef) { }
    }
}
