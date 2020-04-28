using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ParserawkaCore.Model
{
    class ModifiesTable : IModifiesTable
    {
        private List<StatementModify> StatementModifyList = new List<StatementModify>();
        private List<ProcedureModify> ProcedureModifyList = new List<ProcedureModify>();

        public IVariableList GetModifiedBy(Procedure procedure)
        {
            List<ProcedureModify> list = ProcedureModifyList.Where(x => x.Procedure == procedure).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();

            foreach (ProcedureModify procedureModify in list)
                variableList.AddVariable(procedureModify.Variable);

            return variableList;
        }

        public IVariableList GetModifiedBy(Statement statement)
        {
            List<StatementModify> list = StatementModifyList.Where(x => x.Statement == statement).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();

            foreach (StatementModify statementModify in list)
                variableList.AddVariable(statementModify.Variable);

            return variableList;
        }

        public IProcedureList GetModifiesProcedures(Variable variable)
        {
            List<ProcedureModify> list = ProcedureModifyList.Where(x => x.Variable == variable).ToList();
            IProcedureList procedureList = ImplementationFactory.CreateProcedureList();

            foreach (ProcedureModify procedureModify in list)
                procedureList.AddProcedure(procedureModify.Procedure);

            return procedureList;
        }

        public IStatementList GetModifiesStatements(Variable variable)
        {
            List<StatementModify> list = StatementModifyList.Where(x => x.Variable == variable).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList(); 

            foreach (StatementModify statementModify in list)
                statementList.AddStatement(statementModify.Statement);
            
            return statementList;
        }
        
        public bool IsModifies(Statement statement, Variable variable)
        {
            IStatementList statementList = GetModifiesStatements(variable);
            return statementList.Contains(statement);
        }

        public bool IsModifies(Procedure procedure, Variable variable)
        {
            IProcedureList procedureList = GetModifiesProcedures(variable);
            return procedureList.Contains(procedure);
        }

        public void SetModifies(Procedure procedure, Variable variable)
        {
            if (!IsModifies(procedure, variable))
                ProcedureModifyList.Add(new ProcedureModify(procedure, variable));
        }

        public void SetModifies(Statement statement, Variable variable)
        {
            if (!IsModifies(statement, variable))
                StatementModifyList.Add(new StatementModify(statement, variable));
        }
    }
}
