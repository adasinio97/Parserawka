using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlCompare : PqlAst
    {
        public PqlAttrRef LeftRef { get; set; }
        public PqlArgument RightRef { get; set; }

        public PqlCompare(PqlAttrRef leftRef, PqlArgument rightRef)
        {
            LeftRef = leftRef;
            RightRef = rightRef;
        }
    }
}
