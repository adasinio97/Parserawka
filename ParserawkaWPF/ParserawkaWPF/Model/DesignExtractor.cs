using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
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

        public void ExtractData(AST root)
        {
            // Na tę chwilę, zakładam, że korzeń drzewa to procedura.
            // TODO: W późniejszych iteracjach trzeba będzie zmienić na program.
            if (root is IProcedureList)
            {
                foreach (Procedure procedure in root as IProcedureList)
                    ExtractProcedure(procedure);
            }
        }

        private void ExtractProcedure(Procedure procedure)
        {
            IStatementList children = procedure.Body;

            // TODO: Bardziej szczegółowa analiza procedury w późniejszych iteracjach.
            for (int i = 0; i < children.GetSize(); i++)
            {
                Statement child = children[i];
                ExtractStatement(child);
                if (i > 0)
                {
                    Statement previousChild = children[i - 1];
                    FollowsTable.SetFollows(previousChild, child);
                }
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
            IStatementList children = loop.Body;

            for (int i = 0; i < children.GetSize(); i++)
            {
                Statement child = children[i];
                ExtractStatementWithContext(child, loop);

                ParentTable.SetParent(loop, child);
                if (i > 0)
                {
                    Statement previousChild = children[i - 1];
                    FollowsTable.SetFollows(previousChild, child);
                }
            }
        }

        private void ExtractAssign(Assign assign)
        {
            ExtractVariable(assign.Left);
            ExtractFactorWithContext(assign.Right, assign);
            
            ModifiesTable.SetModifies(assign, assign.Left);
        }

        private void ExtractVariable(Variable variable)
        {
            Variables.AddVariable(variable);
        }

        private void ExtractVariableWithContext(Variable variable, Assign context)
        {
            ExtractVariable(variable);
            UsesTable.SetUses(context, variable);
        }

        private void ExtractVariableWithContext(Variable variable, Assign context1, While context2)
        {
            ExtractVariable(variable);
            UsesTable.SetUses(context1, variable);
            UsesTable.SetUses(context2, variable);
        }

        private void ExtractAssignWithContext(Assign assign, While context)
        {
            ExtractVariable(assign.Left);
            ExtractFactorWithContext(assign.Right, assign, context);
            
            ModifiesTable.SetModifies(assign, assign.Left);
            ModifiesTable.SetModifies(context, assign.Left);
        }

        private void ExtractFactorWithContext(Factor factor, Assign context)
        {
            if (factor is Variable)
                ExtractVariableWithContext(factor as Variable, context);
            else if (factor is Expression)
                ExtractExpressionWithContext(factor as Expression, context);
        }

        private void ExtractFactorWithContext(Factor factor, Assign context1, While context2)
        {
            if (factor is Variable)
                ExtractVariableWithContext(factor as Variable, context1, context2);
            else if (factor is Expression)
                ExtractExpressionWithContext(factor as Expression, context1, context2);
        }

        private void ExtractExpressionWithContext(Expression expression, Assign context)
        {
            if (expression.Left is Variable)
                ExtractVariableWithContext(expression.Left as Variable, context);
            else if (expression.Left is Expression)
                ExtractExpressionWithContext(expression.Left as Expression, context);

            if (expression.Right is Variable)
                ExtractVariableWithContext(expression.Right as Variable, context);
            else if (expression.Right is Expression)
                ExtractExpressionWithContext(expression.Right as Expression, context);              
        }

        private void ExtractExpressionWithContext(Expression expression, Assign context1, While context2)
        {
            if (expression.Left is Expression)
                ExtractVariableWithContext(expression.Left as Variable, context1, context2);
            else if (expression.Left is Expression)
                ExtractExpressionWithContext(expression.Left as Expression, context1, context2);

            if (expression.Right is Variable)
                ExtractVariableWithContext(expression.Right as Variable, context1, context2);
            else if (expression.Right is Expression)
                ExtractExpressionWithContext(expression.Right as Expression, context1, context2);
        }
    }
}
