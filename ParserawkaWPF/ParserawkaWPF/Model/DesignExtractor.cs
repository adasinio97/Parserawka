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
            if (root is AstStmtLst)
                ExtractProcedure((root as AstStmtLst).children.FirstOrDefault() as AstProcedure);
        }

        private void ExtractProcedure(AstProcedure procedure)
        {
            List<AST> children = (procedure.Body as AstStmtLst).children;

            // TODO: Bardziej szczegółowa analiza procedury w późniejszych iteracjach.
            for (int i = 0; i < children.Count; i++)
            {
                AstStatement child = children[i] as AstStatement;
                ExtractStatement(child);
                if (i > 0)
                {
                    AstStatement previousChild = children[i - 1] as AstStatement;
                    Statement convertedChild = Statements.GetStatementByProgramLine(child.ProgramLine);
                    Statement convertedPreviousChild = Statements.GetStatementByProgramLine(previousChild.ProgramLine);
                    FollowsTable.SetFollows(convertedPreviousChild, convertedChild);
                }
            }
        }
        
        private void ExtractStatement(AstStatement statement)
        {
            if (statement is AstWhileStatement)
                ExtractWhile(statement as AstWhileStatement);
            else if (statement is AstAssign)
                ExtractAssign(statement as AstAssign);
        }

        private void ExtractStatementWithContext(AstStatement statement, While context)
        {
            if (statement is AstWhileStatement)
                ExtractWhile(statement as AstWhileStatement);
            else if (statement is AstAssign)
                ExtractAssignWithContext(statement as AstAssign, context);
        }

        private void ExtractWhile(AstWhileStatement loop)
        {
            While convertedLoop = new While(loop);
            Statements.AddStatement(convertedLoop);
            List<AST> children = (loop.body as AstStmtLst).children;

            for (int i = 0; i < children.Count; i++)
            {
                AstStatement child = children[i] as AstStatement;
                ExtractStatementWithContext(child, convertedLoop);

                Statement convertedChild = Statements.GetStatementByProgramLine(child.ProgramLine);
                ParentTable.SetParent(convertedLoop, convertedChild);
                if (i > 0)
                {
                    AstStatement previousChild = children[i - 1] as AstStatement;
                    Statement convertedPreviousChild = Statements.GetStatementByProgramLine(previousChild.ProgramLine);
                    FollowsTable.SetFollows(convertedPreviousChild, convertedChild);
                }
            }
        }

        private void ExtractAssign(AstAssign assign)
        {
            Assign convertedAssign = new Assign(assign);
            Statements.AddStatement(convertedAssign);

            ExtractVariable(assign.Left);
            if (assign.Right is AstVariable)
                ExtractVariable(assign.Right as AstVariable);
            else if (assign.Right is AstBinOp)
                ExtractExpressionWithContext(assign.Right as AstBinOp, convertedAssign);

            Variable convertedVariable = Variables.GetVariableByName(assign.Left.name);
            ModifiesTable.SetModifies(convertedAssign, convertedVariable);
        }

        private void ExtractVariable(AstVariable variable)
        {
            Variable convertedVariable = new Variable(variable);
            Variables.AddVariable(convertedVariable);
        }

        private void ExtractVariableWithContext(AstVariable variable, Assign context)
        {
            ExtractVariable(variable);
            Variable convertedVariable = Variables.GetVariableByName(variable.name);
            UsesTable.SetUses(context, convertedVariable);
        }

        private void ExtractVariableWithContext(AstVariable variable, Assign context1, While context2)
        {
            ExtractVariable(variable);
            Variable convertedVariable = Variables.GetVariableByName(variable.name);
            UsesTable.SetUses(context1, convertedVariable);
            UsesTable.SetUses(context2, convertedVariable);
        }

        private void ExtractAssignWithContext(AstAssign assign, While context)
        {
            ExtractVariable(assign.Left);
            Variable convertedVariable = Variables.GetVariableByName(assign.Left.name);
            Statement convertedAssign = Statements.GetStatementByProgramLine(assign.ProgramLine);
            ModifiesTable.SetModifies(convertedAssign, convertedVariable);
            ModifiesTable.SetModifies(context, convertedVariable);
        }

        private void ExtractExpressionWithContext(AstBinOp expression, Assign context)
        {
            if (expression.Left is AstVariable)
                ExtractVariableWithContext(expression.Left as AstVariable, context);
            else if (expression.Left is AstBinOp)
                ExtractExpressionWithContext(expression.Left as AstBinOp, context);
            if (expression.Right is AstVariable)
                ExtractVariableWithContext(expression.Right as AstVariable, context);
            else if (expression.Right is AstBinOp)
                ExtractExpressionWithContext(expression.Right as AstBinOp, context);              
        }

        private void ExtractExpressionWithContext(AstBinOp expression, Assign context1, While context2)
        {
            if (expression.Left is AstVariable)
                ExtractVariableWithContext(expression.Left as AstVariable, context1, context2);
            else if (expression.Left is AstBinOp)
                ExtractExpressionWithContext(expression.Left as AstBinOp, context1, context2);
            if (expression.Right is AstVariable)
                ExtractVariableWithContext(expression.Right as AstVariable, context1, context2);
            else if (expression.Right is AstBinOp)
                ExtractExpressionWithContext(expression.Right as AstBinOp, context1, context2);
        }
    }
}
