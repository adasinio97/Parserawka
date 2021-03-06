﻿using ParserawkaCore.Model;

namespace ParserawkaCore.Interfaces
{
    public interface IUsesTable
    {
        void SetUses(Statement statement, Variable variable);
        void SetUses(Procedure procedure, Variable variable);
        IStatementList GetUsesStatements(Variable variable);
        IProcedureList GetUsesProcedures(Variable variable);
        IVariableList GetUsedBy(Statement statement);
        IVariableList GetUsedBy(Procedure procedure);
        bool IsUses(Statement statement, Variable variable);
        bool IsUses(Procedure procedure, Variable variable);
    }
}
