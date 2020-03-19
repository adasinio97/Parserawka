using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    interface IUsesTable
    {
        void SetUses(Statement statement, Variable variable);
        IStatementList GetUses(Variable variable);
        IVariableList GetUsedBy(Statement statement);
        bool IsUses(Statement statement, Variable variable);
    }
}
