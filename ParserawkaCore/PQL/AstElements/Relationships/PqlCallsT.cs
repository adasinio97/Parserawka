using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlCallsT : PqlCalls
    {
        public PqlCallsT(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Procedure procedure1 = arg1 as Procedure, procedure2 = arg2 as Procedure;
            return pkb.CallsTable.IsCallsT(procedure1, procedure2);
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return pkb.CallsTable.GetCalledByT(procedure);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return pkb.CallsTable.GetCallingT(procedure);
        }
    }
}
