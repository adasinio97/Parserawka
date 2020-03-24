using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IModifiesTable
    {
        void SetModifies(Statement statement, Variable variable);
        void SetModifies(IStatementList statementList, Variable variable);
        IStatementList GetModifies(Variable variable);
        IVariableList GetModifiedBy(Statement statement);
        bool IsModified(Statement statement, Variable variable);
        bool IsModified(IStatementList statementList, Variable variable);
    }
}
