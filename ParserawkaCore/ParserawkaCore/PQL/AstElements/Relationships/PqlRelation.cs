using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlRelation : PqlAst
    {
        public PqlArgument LeftRef;
        public PqlArgument RightRef;

        public PqlTokenType RelationType { get; set; }

        public PqlRelation(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef)
        {
            RelationType = relationType;
            LeftRef = leftRef;
            RightRef = rightRef;
        }
    }
}
