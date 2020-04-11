using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlCallsT : PqlAst
    {
		public PqlAst LeftRef;
		public PqlAst RightRef;
		
		public PqlCallsT(PqlAst leftRef, PqlAst rightRef)
		{
			LeftRef = leftRef;
			RightRef = rightRef;
		}
    }
}
