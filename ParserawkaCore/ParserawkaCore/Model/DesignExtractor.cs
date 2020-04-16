﻿using ParserawkaCore.Interfaces;
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
        public IFollowsTable FollowsTable { get; private set; }
        public IModifiesTable ModifiesTable { get; private set; }
        public IParentTable ParentTable { get; private set; }
        public IUsesTable UsesTable { get; private set; }
        public ICallsTable CallsTable { get; private set; }
        
        private Dictionary<Procedure, List<Call>> calls;

        public DesignExtractor()
        {
            Statements = ImplementationFactory.CreateStatementList();
            Variables = ImplementationFactory.CreateVariableList();
            Procedures = ImplementationFactory.CreateProcedureList();
            FollowsTable = ImplementationFactory.CreateFollowsTable();
            ModifiesTable = ImplementationFactory.CreateModifiesTable();
            ParentTable = ImplementationFactory.CreateParentTable();
            UsesTable = ImplementationFactory.CreateUsesTable();
            CallsTable = ImplementationFactory.CreateCallsTable();

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
            ExtractBody(loop, loop.Body, procedureContext);
        }

        private void ExtractIf(If check, Procedure procedureContext)
        {
            ExtractBody(check, check.IfBody, procedureContext);
            if (check.ElseBody != null)
                ExtractBody(check, check.ElseBody, procedureContext);
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
            
            ModifiesTable.SetModifies(assign, assign.Left);
        }

        private void ExtractVariable(Variable variable)
        {
            Variables.AddVariable(variable);
        }

        private void ExtractVariable(Variable variable, Assign assignContext)
        {
            ExtractVariable(variable);
            UsesTable.SetUses(assignContext, variable);
        }

        private void ExtractFactor(Factor factor, Assign assignContext)
        {
            if (factor is Variable)
                ExtractVariable(factor as Variable, assignContext);
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

            if (calls[procedureContext] == null)
                calls[procedureContext] = new List<Call>();
            calls[procedureContext].Add(call);
        }

        private void ExtractProcedureCalls(Procedure procedure)
        {
            IProcedureList callingProcedures = CallsTable.GetCalling(procedure);
            foreach (Procedure callingProcedure in callingProcedures)
            {
                Call call = calls[callingProcedure].Where(x => x.Procedure == procedure).FirstOrDefault();
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
}