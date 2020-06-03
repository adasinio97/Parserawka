using ParserawkaCore.Model;

namespace ParserawkaCore.Interfaces
{
    public interface IParentTable
    {
        void SetParent(Statement firstStatement, Statement secondStatement);
        Statement GetParent(Statement statement);
        IStatementList GetParentT(Statement statement);
        IStatementList GetParentedBy(Statement statement);
        IStatementList GetParentedByT(Statement statement);
        bool IsParent(Statement firstStatement, Statement secondStatement);
        bool IsParentT(Statement firstStatement, Statement secondStatement);
    }
}
