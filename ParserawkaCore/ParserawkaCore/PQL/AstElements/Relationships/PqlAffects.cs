using System;
using System.Collections.Generic;
using System.Text;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.Utils;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlAffects : PqlRelation
    {
        public PqlAffects(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Assign assignment1 = arg1 as Assign, assignment2 = arg2 as Assign;
            return pkb.AffectsTable.IsAffects(assignment1, assignment2);
        }

        protected override IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Statements.Copy().FilterByType(typeof(Assign));
        }

        protected override IEntityList LoadRightArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Statements.Copy().FilterByType(typeof(Assign));
        }

        protected override IEntityList LoadSingleLeftArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity = pkb.Statements.GetEntityByAttribute((LeftRef as PqlInteger).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity = pkb.Statements.GetEntityByAttribute((RightRef as PqlInteger).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Assign assignment = arg as Assign;
            return pkb.AffectsTable.GetAffectedBy(assignment);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Assign assignment = arg as Assign;
            return pkb.AffectsTable.GetAffects(assignment);
        }
    }
}
