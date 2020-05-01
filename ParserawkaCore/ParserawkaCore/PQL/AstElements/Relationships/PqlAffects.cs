using System;
using System.Collections.Generic;
using System.Text;
using ParserawkaCore.Interfaces;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlAffects : PqlRelation
    {
        public PqlAffects(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList LoadRightArgs(IProgramKnowledgeBase pkb)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList LoadSingleLeftArg(IProgramKnowledgeBase pkb)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            throw new NotImplementedException();
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            throw new NotImplementedException();
        }
    }
}
