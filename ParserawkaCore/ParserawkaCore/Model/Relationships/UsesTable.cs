using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ParserawkaCore.Model
{
    class UsesTable : IUsesTable
    {
        private List<StatementUses> StatementUsesList = new List<StatementUses>();
        private List<ProcedureUses> ProcedureUsesList = new List<ProcedureUses>();

        public IVariableList GetUsedBy(Procedure procedure)
        {
            List<ProcedureUses> list = ProcedureUsesList.Where(x => x.Procedure == procedure).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();
            
            foreach (ProcedureUses procedureUses in list)
                variableList.AddVariable(procedureUses.Variable);

            return variableList;
        }

        public IVariableList GetUsedBy(Statement statement)
        {
            List<StatementUses> list = StatementUsesList.Where(x => x.Statement == statement).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();

            foreach (StatementUses statementUses in list)
                variableList.AddVariable(statementUses.Variable);
            
            return variableList;
        }

        public IProcedureList GetUsesProcedures(Variable variable)
        {
            List<ProcedureUses> list = ProcedureUsesList.Where(x => x.Variable == variable).ToList();
            IProcedureList procedureList = ImplementationFactory.CreateProcedureList();

            foreach (ProcedureUses procedureUses in list)
                procedureList.AddProcedure(procedureUses.Procedure);

            return procedureList;
        }

        public IStatementList GetUsesStatements(Variable variable)
        {
            List<StatementUses> list =  StatementUsesList.Where(x => x.Variable == variable).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            foreach (StatementUses statementUses in list)
                statementList.AddStatement(statementUses.Statement);
            
            return statementList;
        }

        public bool IsUses(Statement statement, Variable variable)
        {
            IStatementList statementList = GetUsesStatements(variable);
            return statementList.Contains(statement);
        }

        public bool IsUses(Procedure procedure, Variable variable)
        {
            IProcedureList procedureList = GetUsesProcedures(variable);
            return procedureList.Contains(procedure);
        }

        public void SetUses(Procedure procedure, Variable variable)
        {
            if (!IsUses(procedure, variable))
                ProcedureUsesList.Add(new ProcedureUses(procedure, variable));
        }

        public void SetUses(Statement statement, Variable variable)
        {
            if (!IsUses(statement, variable))
                StatementUsesList.Add(new StatementUses(statement, variable));
        }
    }
}
