using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL
{
    public class PqlRelationProcessor
    {
        private IProgramKnowledgeBase PKB { get; set; }

        public PqlRelationProcessor(IProgramKnowledgeBase pkb)
        {
            PKB = pkb;
        }

        public IEntity SelectEntityFromPKBLeft(PqlTokenType relationType, PqlArgument argument)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                case PqlTokenType.PARENTT:
                case PqlTokenType.FOLLOWS:
                case PqlTokenType.FOLLOWST:
                case PqlTokenType.NEXT:
                case PqlTokenType.NEXTT:
                case PqlTokenType.AFFECTS:
                case PqlTokenType.AFFECTST:
                    return PKB.Statements.GetEntityByAttribute((argument as PqlInteger).Value);
                case PqlTokenType.MODIFIES:
                case PqlTokenType.USES:
                    if (argument is PqlInteger)
                        return PKB.Statements.GetEntityByAttribute((argument as PqlInteger).Value);
                    else
                        return PKB.Procedures.GetEntityByAttribute((argument as PqlString).Value);
                case PqlTokenType.CALLS:
                case PqlTokenType.CALLST:
                    return PKB.Procedures.GetEntityByAttribute((argument as PqlString).Value);
            }
            return null;
        }

        public IEntity SelectEntityFromPKBRight(PqlTokenType relationType, PqlArgument argument)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                case PqlTokenType.PARENTT:
                case PqlTokenType.FOLLOWS:
                case PqlTokenType.FOLLOWST:
                case PqlTokenType.NEXT:
                case PqlTokenType.NEXTT:
                case PqlTokenType.AFFECTS:
                case PqlTokenType.AFFECTST:
                    return PKB.Statements.GetEntityByAttribute((argument as PqlInteger).Value);
                case PqlTokenType.MODIFIES:
                case PqlTokenType.USES:
                    return PKB.Variables.GetEntityByAttribute((argument as PqlString).Value);
                case PqlTokenType.CALLS:
                case PqlTokenType.CALLST:
                    return PKB.Procedures.GetEntityByAttribute((argument as PqlString).Value);
            }
            return null;
        }

        public IEntityList SelectListFromPKBLeft(PqlTokenType relationType)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                case PqlTokenType.PARENTT:
                case PqlTokenType.FOLLOWS:
                case PqlTokenType.FOLLOWST:
                case PqlTokenType.NEXT:
                case PqlTokenType.NEXTT:
                case PqlTokenType.AFFECTS:
                case PqlTokenType.AFFECTST:
                    return PKB.Statements.Copy();
                case PqlTokenType.MODIFIES:
                case PqlTokenType.USES:
                    return PKB.Procedures.Copy().Sum(PKB.Statements);
                case PqlTokenType.CALLS:
                case PqlTokenType.CALLST:
                    return PKB.Procedures.Copy();
            }
            return null;
        }

        public IEntityList SelectListFromPKBRight(PqlTokenType relationType)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                case PqlTokenType.PARENTT:
                case PqlTokenType.FOLLOWS:
                case PqlTokenType.FOLLOWST:
                case PqlTokenType.NEXT:
                case PqlTokenType.NEXTT:
                case PqlTokenType.AFFECTS:
                case PqlTokenType.AFFECTST:
                    return PKB.Statements.Copy();
                case PqlTokenType.MODIFIES:
                case PqlTokenType.USES:
                    return PKB.Variables.Copy();
                case PqlTokenType.CALLS:
                case PqlTokenType.CALLST:
                    return PKB.Procedures.Copy();
            }
            return null;
        }

        public bool RelationFull(PqlTokenType relationType, IEntity arg1, IEntity arg2)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                    return ParentRelFull(arg1, arg2);
                case PqlTokenType.PARENTT:
                    return ParentTRelFull(arg1, arg2);
                case PqlTokenType.FOLLOWS:
                    return FollowsRelFull(arg1, arg2);
                case PqlTokenType.FOLLOWST:
                    return FollowsTRelFull(arg1, arg2);
                case PqlTokenType.MODIFIES:
                    return ModifiesRelFull(arg1, arg2);
                case PqlTokenType.USES:
                    return UsesRelFull(arg1, arg2);
                case PqlTokenType.CALLS:
                    return CallsRelFull(arg1, arg2);
                case PqlTokenType.CALLST:
                    return CallsTRelFull(arg1, arg2);
                case PqlTokenType.NEXT:
                    return NextRelFull(arg1, arg2);
                case PqlTokenType.NEXTT:
                    return NextTRelFull(arg1, arg2);
                case PqlTokenType.AFFECTS:
                    return AffectsRelFull(arg1, arg2);
                case PqlTokenType.AFFECTST:
                    return AffectsTRelFull(arg1, arg2);
            }
            return false;
        }

        public IEntityList RelationLeft(PqlTokenType relationType, IEntity arg)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                    return ParentRelLeft(arg);
                case PqlTokenType.PARENTT:
                    return ParentTRelLeft(arg);
                case PqlTokenType.FOLLOWS:
                    return FollowsRelLeft(arg);
                case PqlTokenType.FOLLOWST:
                    return FollowsTRelLeft(arg);
                case PqlTokenType.MODIFIES:
                    return ModifiesRelLeft(arg);
                case PqlTokenType.USES:
                    return UsesRelLeft(arg);
                case PqlTokenType.CALLS:
                    return CallsRelLeft(arg);
                case PqlTokenType.CALLST:
                    return CallsTRelLeft(arg);
                case PqlTokenType.NEXT:
                    return NextRelLeft(arg);
                case PqlTokenType.NEXTT:
                    return NextTRelLeft(arg);
                case PqlTokenType.AFFECTS:
                    return AffectsRelLeft(arg);
                case PqlTokenType.AFFECTST:
                    return AffectsTRelLeft(arg);
            }
            return null;
        }

        public IEntityList RelationRight(PqlTokenType relationType, IEntity arg)
        {
            switch (relationType)
            {
                case PqlTokenType.PARENT:
                    return ParentRelRight(arg);
                case PqlTokenType.PARENTT:
                    return ParentTRelRight(arg);
                case PqlTokenType.FOLLOWS:
                    return FollowsRelRight(arg);
                case PqlTokenType.FOLLOWST:
                    return FollowsTRelRight(arg);
                case PqlTokenType.MODIFIES:
                    return ModifiesRelRight(arg);
                case PqlTokenType.USES:
                    return UsesRelRight(arg);
                case PqlTokenType.CALLS:
                    return CallsRelRight(arg);
                case PqlTokenType.CALLST:
                    return CallsTRelRight(arg);
                case PqlTokenType.NEXT:
                    return NextRelRight(arg);
                case PqlTokenType.NEXTT:
                    return NextTRelRight(arg);
                case PqlTokenType.AFFECTS:
                    return AffectsRelRight(arg);
                case PqlTokenType.AFFECTST:
                    return AffectsTRelRight(arg);
            }
            return null;
        }

        private bool ParentRelFull(IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return PKB.ParentTable.IsParent(statement1, statement2);
        }

        private IEntityList ParentRelLeft(IEntity arg)
        {
            Statement statement = arg as Statement;
            return PKB.ParentTable.GetParentedBy(statement);
        }

        private IEntityList ParentRelRight(IEntity arg)
        {
            Statement statement = arg as Statement;
            Statement parent = PKB.ParentTable.GetParent(statement);
            IStatementList result = ImplementationFactory.CreateStatementList();
            result.AddStatement(parent);
            return result;
        }

        private bool ParentTRelFull(IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return PKB.ParentTable.IsParentT(statement1, statement2);
        }
        
        private IEntityList ParentTRelLeft(IEntity arg)
        {
            Statement statement = arg as Statement;
            return PKB.ParentTable.GetParentedByT(statement);
        }

        private IEntityList ParentTRelRight(IEntity arg)
        {
            Statement statement = arg as Statement;
            return PKB.ParentTable.GetParentT(statement);
        }

        private bool FollowsRelFull(IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return PKB.FollowsTable.IsFollows(statement1, statement2);
        }

        private IEntityList FollowsRelLeft(IEntity arg)
        {
            Statement statement = arg as Statement;
            Statement follows = PKB.FollowsTable.GetFollows(statement);
            IStatementList result = ImplementationFactory.CreateStatementList();
            result.AddStatement(follows);
            return result;
        }

        private IEntityList FollowsRelRight(IEntity arg)
        {
            Statement statement = arg as Statement;
            Statement followed = PKB.FollowsTable.GetFollowedBy(statement);
            IStatementList result = ImplementationFactory.CreateStatementList();
            result.AddStatement(followed);
            return result;
        }

        private bool FollowsTRelFull(IEntity arg1, IEntity arg2)
        {
            Statement statement1 = arg1 as Statement, statement2 = arg2 as Statement;
            return PKB.FollowsTable.IsFollowsT(statement1, statement2);
        }

        private IEntityList FollowsTRelLeft(IEntity arg)
        {
            Statement statement = arg as Statement;
            return PKB.FollowsTable.GetFollowsT(statement);
        }

        private IEntityList FollowsTRelRight(IEntity arg)
        {
            Statement statement = arg as Statement;
            return PKB.FollowsTable.GetFollowedByT(statement);
        }

        private bool ModifiesRelFull(IEntity arg1, IEntity arg2)
        {
            Variable variable = arg2 as Variable;
            if (arg1 is Procedure)
            {
                Procedure procedure = arg1 as Procedure;
                return PKB.ModifiesTable.IsModifies(procedure, variable);
            }
            else
            {
                Statement statement = arg1 as Statement;
                return PKB.ModifiesTable.IsModifies(statement, variable);
            }
        }

        private IEntityList ModifiesRelLeft(IEntity arg)
        {
            if (arg is Procedure)
            {
                Procedure procedure = arg as Procedure;
                return PKB.ModifiesTable.GetModifiedBy(procedure);
            }
            else
            {
                Statement statement = arg as Statement;
                return PKB.ModifiesTable.GetModifiedBy(statement);
            }
        }

        private IEntityList ModifiesRelRight(IEntity arg)
        {
            Variable variable = arg as Variable;
            return PKB.ModifiesTable.GetModifiesProcedures(variable).Sum(PKB.ModifiesTable.GetModifiesStatements(variable));
        }

        private bool UsesRelFull(IEntity arg1, IEntity arg2)
        {
            Variable variable = arg2 as Variable;
            if (arg1 is Procedure)
            {
                Procedure procedure = arg1 as Procedure;
                return PKB.UsesTable.IsUses(procedure, variable);
            }
            else
            {
                Statement statement = arg1 as Statement;
                return PKB.UsesTable.IsUses(statement, variable);
            }
        }

        private IEntityList UsesRelLeft(IEntity arg)
        {
            if (arg is Procedure)
            {
                Procedure procedure = arg as Procedure;
                return PKB.UsesTable.GetUsedBy(procedure);
            }
            else
            {
                Statement statement = arg as Statement;
                return PKB.UsesTable.GetUsedBy(statement);
            }
        }

        private IEntityList UsesRelRight(IEntity arg)
        {
            Variable variable = arg as Variable;
            return PKB.UsesTable.GetUsesProcedures(variable).Sum(PKB.UsesTable.GetUsesStatements(variable));
        }

        private bool CallsRelFull(IEntity arg1, IEntity arg2)
        {
            Procedure procedure1 = arg1 as Procedure, procedure2 = arg2 as Procedure;
            return PKB.CallsTable.IsCalls(procedure1, procedure2);
        }

        private IEntityList CallsRelLeft(IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return PKB.CallsTable.GetCalledBy(procedure);
        }

        private IEntityList CallsRelRight(IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return PKB.CallsTable.GetCalling(procedure);
        }

        private bool CallsTRelFull(IEntity arg1, IEntity arg2)
        {
            Procedure procedure1 = arg1 as Procedure, procedure2 = arg2 as Procedure;
            return PKB.CallsTable.IsCallsT(procedure1, procedure2);
        }

        private IEntityList CallsTRelLeft(IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return PKB.CallsTable.GetCalledByT(procedure);
        }

        private IEntityList CallsTRelRight(IEntity arg)
        {
            Procedure procedure = arg as Procedure;
            return PKB.CallsTable.GetCalling(procedure);
        }

        private bool NextRelFull(IEntity arg1, IEntity arg2)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList NextRelLeft(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList NextRelRight(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private bool NextTRelFull(IEntity arg1, IEntity arg2)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList NextTRelLeft(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList NextTRelRight(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private bool AffectsRelFull(IEntity arg1, IEntity arg2)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList AffectsRelLeft(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList AffectsRelRight(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private bool AffectsTRelFull(IEntity arg1, IEntity arg2)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList AffectsTRelLeft(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }

        private IEntityList AffectsTRelRight(IEntity arg)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
