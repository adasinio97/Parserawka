﻿using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlNextT : PqlNext
    {
        public PqlNextT(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return pkb.NextTable.IsNextT(statement1, statement2);
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Statement statement = arg as Statement;
            return pkb.NextTable.GetNextT(statement);
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Statement statement = arg as Statement;
            return pkb.NextTable.GetNextedByT(statement);
        }
    }
}
