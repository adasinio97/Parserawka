using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ParserawkaCore.Model
{
    class UsesTable : IUsesTable
    {
        public IVariableList GetUsedBy(Procedure procedure)
        {
            if (procedure != null)
                return procedure.Using.Copy();
            else
                return ImplementationFactory.CreateVariableList();
        }

        public IVariableList GetUsedBy(Statement statement)
        {
            if (statement != null)
                return statement.Using.Copy();
            else
                return ImplementationFactory.CreateVariableList();
        }

        public IProcedureList GetUsesProcedures(Variable variable)
        {
            if (variable != null)
                return variable.UsedByProcedures;
            else
                return ImplementationFactory.CreateProcedureList();
        }

        public IStatementList GetUsesStatements(Variable variable)
        {
            if (variable != null)
                return variable.UsedByStatements;
            else
                return ImplementationFactory.CreateStatementList();
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
            procedure.Using.AddVariable(variable);
            variable.UsedByProcedures.AddProcedure(procedure);
        }

        public void SetUses(Statement statement, Variable variable)
        {
            statement.Using.AddVariable(variable);
            variable.UsedByStatements.AddStatement(statement);
        }
    }
}
