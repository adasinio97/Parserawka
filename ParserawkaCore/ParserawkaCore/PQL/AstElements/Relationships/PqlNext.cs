using System;
using System.Collections.Generic;
using System.Text;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.Utils;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlNext : PqlRelation
    {
        public PqlNext(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return pkb.NextTable.IsNext(statement1, statement2);
        }

        protected override IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Statements.Copy();
        }

        protected override IEntityList LoadRightArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Statements.Copy();
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
            Statement statement = arg as Statement;
            return pkb.NextTable.GetNext(statement);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Statement statement = arg as Statement;
            return pkb.NextTable.GetNextedBy(statement);
        }
    }
}
