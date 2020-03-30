using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IFollowsTable
    {
        void SetFollows(Statement firstStatement, Statement secondStatement);
        Statement GetFollows(Statement statement);
        IStatementList GetFollowsT(Statement statement, IStatementList statementList);
        Statement GetFollowedBy(Statement statement);
        IStatementList GetFollowedByT(Statement statement, IStatementList statementList);
        bool IsFollows(Statement firstStatement, Statement secondStatement);
        bool IsFollowsT(Statement firstStatement, Statement secondStatement);
    }
}
