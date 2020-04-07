using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IModifiesTable
    {
        void SetModifies(Statement statement, Variable variable);
        void SetModifies(Procedure procedure, Variable variable);
        IStatementList GetModifiesStatements(Variable variable);
        IProcedureList GetModifiesProcedures(Variable variable);
        IVariableList GetModifiedBy(Statement statement);
        IVariableList GetModifiedBy(Procedure procedure);
        bool IsModified(Statement statement, Variable variable);
    }
}
