using System;
using System.Collections.Generic;
using System.Text;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.Utils;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlCalls : PqlRelation
    {
        public PqlCalls(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Procedure procedure1 = arg1 as Procedure, procedure2 = arg2 as Procedure;
            return pkb.CallsTable.IsCalls(procedure1, procedure2);
        }

        protected override IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Procedures.Copy();
        }

        protected override IEntityList LoadRightArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Procedures.Copy();
        }

        protected override IEntityList LoadSingleLeftArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity = pkb.Procedures.GetEntityByAttribute((LeftRef as PqlString).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity = pkb.Procedures.GetEntityByAttribute((RightRef as PqlString).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return pkb.CallsTable.GetCalledBy(procedure);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return pkb.CallsTable.GetCalling(procedure);
        }
    }
}
