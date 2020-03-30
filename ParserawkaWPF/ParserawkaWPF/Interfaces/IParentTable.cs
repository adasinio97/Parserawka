using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IParentTable
    {
        void SetParent(Statement firstStatement, Statement secondStatement);
        Statement GetParent(Statement statement);
        IStatementList GetParentT(Statement statement, IStatementList statementList);
        IStatementList GetParentedBy(Statement statement);
        IStatementList GetParentedByT(Statement statement, IStatementList statementList);
        bool IsParent(Statement firstStatement, Statement secondStatement);
        bool IsParentT(Statement firstStatement, Statement secondStatement, IStatementList statementList);
    }
}
