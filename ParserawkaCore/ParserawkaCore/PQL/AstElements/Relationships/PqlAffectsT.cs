using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlAffectsT : PqlAffects
    {
        public PqlAffectsT(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Assign assignment1 = arg1 as Assign, assignment2 = arg2 as Assign;
            return pkb.AffectsTable.IsAffectsT(assignment1, assignment2);
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Assign assignment = arg as Assign;
            return pkb.AffectsTable.GetAffectedByT(assignment);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Assign assignment = arg as Assign;
            return pkb.AffectsTable.GetAffectsT(assignment);
        }
    }
}
