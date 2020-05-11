using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class DesignExtractor : IDesignExtractor
    {
        public IStatementList Statements { get; private set; }
        public IVariableList Variables { get; private set; }
        public IProcedureList Procedures { get; private set; }
        public IConstantList Constants { get; private set; }
        public IFollowsTable FollowsTable { get; private set; }
        public IModifiesTable ModifiesTable { get; private set; }
        public IParentTable ParentTable { get; private set; }
        public IUsesTable UsesTable { get; private set; }
        public ICallsTable CallsTable { get; private set; }
        public INextTable NextTable { get; private set; }
        public IAffectsTable AffectsTable { get; private set; }
        
        private Dictionary<Procedure, List<Call>> calls;

        public DesignExtractor()
        {
            Statements = ImplementationFactory.CreateStatementList();
            Variables = ImplementationFactory.CreateVariableList();
            Procedures = ImplementationFactory.CreateProcedureList();
            Constants = ImplementationFactory.CreateConstantList();
            FollowsTable = ImplementationFactory.CreateFollowsTable();
            ModifiesTable = ImplementationFactory.CreateModifiesTable();
            ParentTable = ImplementationFactory.CreateParentTable();
            UsesTable = ImplementationFactory.CreateUsesTable();
            CallsTable = ImplementationFactory.CreateCallsTable();
            NextTable = ImplementationFactory.CreateNextTable();
            AffectsTable = ImplementationFactory.CreateAffectsTable();

            calls = new Dictionary<Procedure, List<Call>>();
        }

        public void ExtractData(AST root)
        {
            if (root is IProcedureList)
            {
                foreach (Procedure procedure in root as IProcedureList)
                    Procedures.AddProcedure(procedure);
                foreach (Procedure procedure in root as IProcedureList)
                    ExtractProcedure(procedure);
                
                foreach (Procedure procedure in Procedures)
                {
                    IProcedureList calledProcedures = CallsTable.GetCalledBy(procedure);
                    if (calledProcedures.GetSize() == 0)
                        ExtractProcedureCalls(procedure);
                }

                ExtractAffects();
            }
        }

        private void ExtractProcedure(Procedure procedure)
        {
            IStatementList children = procedure.Body;        
            for (int i = 0; i < children.GetSize(); i++)
            {
                Statement child = children[i];
                ExtractStatement(child, procedure);
                if (i > 0)
                {
                    Statement previousChild = children[i - 1];
                    FollowsTable.SetFollows(previousChild, child);

                    if (!(previousChild is If))
                        NextTable.SetNext(previousChild, child);
                    else
                    {
                        If ifChild = previousChild as If;
                        NextTable.SetNext(ifChild.IfBody.GetLast(), child);
                        if (ifChild.ElseBody != null)
                            NextTable.SetNext(ifChild.ElseBody.GetLast(), child);
                    }
                }

                IVariableList modifiedVariables = ModifiesTable.GetModifiedBy(child);
                IVariableList usedVariables = UsesTable.GetUsedBy(child);

                foreach (Variable variable in modifiedVariables)
                    ModifiesTable.SetModifies(procedure, variable);
                foreach (Variable variable in usedVariables)
                    UsesTable.SetUses(procedure, variable);
            }
        }
        
        private void ExtractStatement(Statement statement, Procedure procedureContext)
        {
            Statements.AddStatement(statement);
            if (statement is While)
                ExtractWhile(statement as While, procedureContext);
            else if (statement is If)
                ExtractIf(statement as If, procedureContext);
            else if (statement is Assign)
                ExtractAssign(statement as Assign);
            else if (statement is Call)
                ExtractCall(statement as Call, procedureContext);
        }

        private void ExtractWhile(While loop, Procedure procedureContext)
        {
            ExtractVariable(loop.Condition);
            Variable retrievedVariable = Variables.GetVariableByName(loop.Condition.Name);
            UsesTable.SetUses(loop, retrievedVariable);
            ExtractBody(loop, loop.Body, procedureContext);

            NextTable.SetNext(loop, loop.Body.GetFirst());
            if (loop.Body.GetLast() is If)
            {
                If check = loop.Body.GetLast() as If;
                NextTable.SetNext(check.IfBody.GetLast(), loop);
                if (check.ElseBody != null)
                    NextTable.SetNext(check.ElseBody.GetLast(), loop);
            }
            else
                NextTable.SetNext(loop.Body.GetLast(), loop);
        }

        private void ExtractIf(If check, Procedure procedureContext)
        {
            ExtractVariable(check.Condition);
            Variable retrievedVariable = Variables.GetVariableByName(check.Condition.Name);
            UsesTable.SetUses(check, retrievedVariable);
            ExtractBody(check, check.IfBody, procedureContext);
            NextTable.SetNext(check, check.IfBody.GetFirst());
            if (check.ElseBody != null)
            {
                ExtractBody(check, check.ElseBody, procedureContext);
                NextTable.SetNext(check, check.ElseBody.GetFirst());
            }
        }

        private void ExtractBody(Container container, IStatementList body, Procedure procedureContext)
        {
            for (int i = 0; i < body.GetSize(); i++)
            {
                Statement child = body[i];
                ExtractStatement(child, procedureContext);

                ParentTable.SetParent(container, child);
                if (i > 0)
                {
                    Statement previousChild = body[i - 1];
                    FollowsTable.SetFollows(previousChild, child);

                    if (!(previousChild is If))
                        NextTable.SetNext(previousChild, child);
                    else
                    {
                        If ifChild = previousChild as If;
                        NextTable.SetNext(ifChild.IfBody.GetLast(), child);
                        if (ifChild.ElseBody != null)
                            NextTable.SetNext(ifChild.ElseBody.GetLast(), child);
                    }
                }

                IVariableList modifiedVariables = ModifiesTable.GetModifiedBy(child);
                IVariableList usedVariables = UsesTable.GetUsedBy(child);

                foreach (Variable variable in modifiedVariables)
                    ModifiesTable.SetModifies(container, variable);
                foreach (Variable variable in usedVariables)
                    UsesTable.SetUses(container, variable);
            }
        }

        private void ExtractAssign(Assign assign)
        {
            ExtractVariable(assign.Left);
            ExtractFactor(assign.Right, assign);

            Variable retrievedVariable = Variables.GetVariableByName(assign.Left.Name);
            ModifiesTable.SetModifies(assign, retrievedVariable);
        }

        private void ExtractVariable(Variable variable)
        {
            Variables.AddVariable(variable);
        }

        private void ExtractVariable(Variable variable, Assign assignContext)
        {
            ExtractVariable(variable);
            Variable retrievedVariable = Variables.GetVariableByName(variable.Name);
            UsesTable.SetUses(assignContext, retrievedVariable);
        }

        private void ExtractConstant(Constant constant)
        {
            Constants.AddConstant(constant);
        }

        private void ExtractFactor(Factor factor, Assign assignContext)
        {
            if (factor is Variable)
                ExtractVariable(factor as Variable, assignContext);
            else if (factor is Constant)
                ExtractConstant(factor as Constant);
            else if (factor is Expression)
                ExtractExpression(factor as Expression, assignContext);
        }

        private void ExtractExpression(Expression expression, Assign assignContext)
        {
            ExtractFactor(expression.Left, assignContext);
            ExtractFactor(expression.Right, assignContext);             
        }

        private void ExtractCall(Call call, Procedure procedureContext)
        {
            Procedure calledProcedure = Procedures.GetProcedureByName(call.ProcedureName);
            call.Procedure = calledProcedure;

            CallsTable.SetCalls(procedureContext, calledProcedure);
            
            if (!calls.ContainsKey(procedureContext)) 
                calls[procedureContext] = new List<Call>();
            calls[procedureContext].Add(call);
        }

        private void ExtractProcedureCalls(Procedure procedure)
        {
            IProcedureList callingProcedures = CallsTable.GetCalling(procedure);
            foreach (Procedure callingProcedure in callingProcedures)
            {
                List<Call> procedureCalls = calls[callingProcedure].Where(x => x.Procedure == procedure).ToList();
                foreach (Call call in procedureCalls)
                {
                    IStatementList callParents = ParentTable.GetParentT(call);

                    IVariableList modifiedVariables = ModifiesTable.GetModifiedBy(procedure);
                    IVariableList usedVariables = UsesTable.GetUsedBy(procedure);

                    foreach (Variable variable in modifiedVariables)
                    {
                        ModifiesTable.SetModifies(call, variable);
                        ModifiesTable.SetModifies(callingProcedure, variable);
                        foreach (Statement parent in callParents)
                            ModifiesTable.SetModifies(parent, variable);
                    }
                    foreach (Variable variable in usedVariables)
                    {
                        UsesTable.SetUses(call, variable);
                        UsesTable.SetUses(callingProcedure, variable);
                        foreach (Statement parent in callParents)
                            UsesTable.SetUses(parent, variable);
                    }
                    ExtractProcedureCalls(callingProcedure);
                }
            }
        }

        private void ExtractAffects()
        {
            IStatementList assignStatements = Statements.Copy().FilterByType(typeof(Assign)) as IStatementList;
            foreach (Statement assignStatement in assignStatements)
            {
                Assign assignment = assignStatement as Assign;
                Variable variable = ModifiesTable.GetModifiedBy(assignment).GetVariableByIndex(0);

                IStatementList nextStatements = NextTable.GetNextT(assignStatement);
                foreach (Statement nextStatement in nextStatements)
                {
                    if (nextStatement is Assign && UsesTable.IsUses(nextStatement, variable))
                        AffectsTable.SetAffects(assignment, nextStatement as Assign);
                    if (nextStatement != assignStatement && (nextStatement is Assign || nextStatement is Call) && ModifiesTable.IsModifies(nextStatement, variable))
                        break;
                }
            }
        }
    }
}
