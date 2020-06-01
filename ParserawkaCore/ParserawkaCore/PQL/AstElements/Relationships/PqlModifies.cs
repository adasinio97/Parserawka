using System;
using System.Collections.Generic;
using System.Text;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.Utils;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlModifies : PqlRelation
    {
        public PqlModifies(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef) : base(relationType, leftRef, rightRef) { }

        public override bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2)
        {
            Variable variable = arg2 as Variable;
            if (arg1 is Procedure)
            {
                Procedure procedure = arg1 as Procedure;
                return pkb.ModifiesTable.IsModifies(procedure, variable);
            }
            else
            {
                Statement statement = arg1 as Statement;
                return pkb.ModifiesTable.IsModifies(statement, variable);
            }
        }

        protected override IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Procedures.Copy().Sum(pkb.Statements);
        }

        protected override IEntityList LoadRightArgs(IProgramKnowledgeBase pkb)
        {
            return pkb.Variables.Copy();
        }

        protected override IEntityList LoadSingleLeftArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity;
            if (LeftRef is PqlInteger)
                entity = pkb.Statements.GetEntityByAttribute((LeftRef as PqlInteger).Value);
            else
                entity = pkb.Procedures.GetEntityByAttribute((LeftRef as PqlString).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity = pkb.Variables.GetEntityByAttribute((RightRef as PqlString).Value);
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }

        protected override IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            if (arg is Procedure)
            {
                Procedure procedure = arg as Procedure;
                return pkb.ModifiesTable.GetModifiedBy(procedure);
            }
            else
            {
                Statement statement = arg as Statement;
                return pkb.ModifiesTable.GetModifiedBy(statement);
            }
        }

        protected override IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg)
        {
            Variable variable = arg as Variable;
            return pkb.ModifiesTable.GetModifiesProcedures(variable)
                .Sum(pkb.ModifiesTable.GetModifiesStatements(variable));
        }
    }
}
