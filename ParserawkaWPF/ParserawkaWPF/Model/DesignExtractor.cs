using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class DesignExtractor : IDesignExtractor
    {
        public IStatementList Statements { get; private set; }
        public IVariableList Variables { get; private set; }
        public IFollowsTable FollowsTable { get; private set; }
        public IModifiesTable ModifiesTable { get; private set; }
        public IParentTable ParentTable { get; private set; }
        public IUsesTable UsesTable { get; private set; }

        public DesignExtractor()
        {
            Statements = ImplementationFactory.CreateStatementList();
            Variables = ImplementationFactory.CreateVariableList();
            FollowsTable = ImplementationFactory.CreateFollowsTable();
            ModifiesTable = ImplementationFactory.CreateModifiesTable();
            ParentTable = ImplementationFactory.CreateParentTable();
            UsesTable = ImplementationFactory.CreateUsesTable();
        }

        public void ExtractData(ITmpAst abstractSyntaxTree)
        {
            // Na tę chwilę, zakładam, że korzeń drzewa to procedura.
            // TODO: W późniejszych iteracjach trzeba będzie zmienić na program.
            if (abstractSyntaxTree is Procedure)
                ExtractProcedure(abstractSyntaxTree as Procedure);
        }

        private void ExtractProcedure(Procedure procedure)
        {
            // TODO: Bardziej szczegółowa analiza procedury w późniejszych iteracjach.
            for (int i = 0; i < procedure.StatementList.GetSize(); i++)
            {
                Statement child = procedure.StatementList.GetStatementByIndex(i);
                if (i > 0)
                {
                    Statement previousChild = procedure.StatementList.GetStatementByIndex(i - 1);
                    FollowsTable.SetFollows(previousChild, child);
                }
                ExtractStatement(child);
            }
        }
        
        private void ExtractStatement(Statement statement)
        {
            Statements.AddStatement(statement);
            if (statement is While)
                ExtractWhile(statement as While);
            else if (statement is Assign)
                ExtractAssign(statement as Assign);
        }

        private void ExtractStatementWithContext(Statement statement, While context)
        {
            Statements.AddStatement(statement);
            if (statement is While)
                ExtractWhile(statement as While);
            else if (statement is Assign)
                ExtractAssignWithContext(statement as Assign, context);
        }

        private void ExtractWhile(While loop)
        {
            for (int i = 0; i < loop.StatementList.GetSize(); i++)
            {
                Statement child = loop.StatementList.GetStatementByIndex(i);
                ParentTable.SetParent(loop, child);
                if (i > 0)
                {
                    Statement previousChild = loop.StatementList.GetStatementByIndex(i - 1);
                    FollowsTable.SetFollows(previousChild, child);
                }
                ExtractStatementWithContext(child, loop);
            }
        }

        private void ExtractAssign(Assign assign)
        {
            ModifiesTable.SetModifies(assign, assign.LeftSide);
            ExtractVariable(assign.LeftSide);
            ExtractExpressionWithContext(assign.RightSide, assign);
        }

        private void ExtractVariable(Variable variable)
        {
            Variables.AddVariable(variable);
        }

        private void ExtractVariableWithContext(Variable variable, Assign context)
        {
            Variables.AddVariable(variable);
            UsesTable.SetUses(context, variable);
        }

        private void ExtractVariableWithContext(Variable variable, Assign context1, While context2)
        {
            Variables.AddVariable(variable);
            UsesTable.SetUses(context1, variable);
            UsesTable.SetUses(context2, variable);
        }

        private void ExtractAssignWithContext(Assign assign, While context)
        {
            ModifiesTable.SetModifies(assign, assign.LeftSide);
            ModifiesTable.SetModifies(context, assign.LeftSide);
            ExtractVariable(assign.LeftSide);
        }

        private void ExtractExpressionWithContext(Expression expression, Assign context)
        {
            if (expression.Factor is Variable)
                ExtractVariableWithContext(expression.Factor as Variable, context);
            if (expression.ChildExpression != null)
                ExtractExpressionWithContext(expression.ChildExpression, context);                
        }

        private void ExtractExpressionWithContext(Expression expression, Assign context1, While context2)
        {
            if (expression.Factor is Variable)
                ExtractVariableWithContext(expression.Factor as Variable, context1, context2);
            if (expression.ChildExpression != null)
                ExtractExpressionWithContext(expression.ChildExpression, context1, context2);
        }
    }
}
