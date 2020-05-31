using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ParserawkaCore.Model
{
    class ModifiesTable : IModifiesTable
    {
        public IVariableList GetModifiedBy(Procedure procedure)
        {
            if (procedure != null)
                return procedure.Modifying.Copy();
            else
                return ImplementationFactory.CreateVariableList();
        }

        public IVariableList GetModifiedBy(Statement statement)
        {
            if (statement != null)
                return statement.Modifying.Copy();
            else
                return ImplementationFactory.CreateVariableList();
        }

        public IProcedureList GetModifiesProcedures(Variable variable)
        {
            if (variable != null)
                return variable.ModifiedByProcedures;
            else
                return ImplementationFactory.CreateProcedureList();
        }

        public IStatementList GetModifiesStatements(Variable variable)
        {
            if (variable != null)
                return variable.ModifiedByStatements;
            else
                return ImplementationFactory.CreateStatementList();
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
            procedure.Modifying.AddVariable(variable);
            variable.ModifiedByProcedures.AddProcedure(procedure);
        }

        public void SetModifies(Statement statement, Variable variable)
        {
            statement.Modifying.AddVariable(variable);
            variable.ModifiedByStatements.AddStatement(statement);
        }
    }
}
