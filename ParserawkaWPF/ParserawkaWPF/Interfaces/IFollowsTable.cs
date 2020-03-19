using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    interface IFollowsTable
    {
        void SetFollows(Statement firstStatement, Statement secondStatement);
        Statement GetFollows(Statement statement);
        IStatementList GetFollowsT(Statement statement);
        Statement GetFollowedBy(Statement statement);
        IStatementList GetFollowedByT(Statement statement);
        bool IsFollows(Statement firstStatement, Statement secondStatement);
        bool IsFollowsT(Statement firstStatement, Statement secondStatement);
    }
}
