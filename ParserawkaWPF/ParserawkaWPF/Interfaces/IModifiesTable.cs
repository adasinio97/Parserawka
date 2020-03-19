using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    interface IModifiesTable
    {
        void SetModifies(Statement statement, Variable variable);
        IStatementList GetModifies(Variable variable);
        IVariableList GetModifiedBy(Statement statement);
        bool IsModified(Statement statement, Variable variable);
    }
}
