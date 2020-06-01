using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaCore.Model
{
    class FollowsTable : IFollowsTable
    {
        public Statement GetFollowedBy(Statement statement)
        {
            return statement?.Following;
        }

        public IStatementList GetFollowedByT(Statement statement)
        {
            IStatementList followed = ImplementationFactory.CreateStatementList();
            Statement followedStatement = GetFollowedBy(statement);
            followed.AddStatement(followedStatement);
            for (int i = 0; i < followed.GetSize(); i++)
                followed.AddStatement(GetFollowedBy(followed[i]));
            return followed;
        }

        public Statement GetFollows(Statement statement)
        {
            return statement?.FollowedBy;
        }

        public IStatementList GetFollowsT(Statement statement)
        {
            IStatementList following = ImplementationFactory.CreateStatementList();
            Statement followingStatement = GetFollows(statement);
            following.AddStatement(followingStatement);
            for (int i = 0; i < following.GetSize(); i++)
                following.AddStatement(GetFollows(following[i]));
            return following;
        }

        public bool IsFollows(Statement firstStatement, Statement secondStatement)
        {
           return firstStatement == GetFollowedBy(secondStatement);
        }

        public bool IsFollowsT(Statement firstStatement, Statement secondStatement)
        {
            IStatementList statementList = GetFollowedByT(secondStatement);
            return statementList.Contains(firstStatement);
        }

        public void SetFollows(Statement firstStatement, Statement secondStatement)
        {
            firstStatement.FollowedBy = secondStatement;
            secondStatement.Following = firstStatement;
        }
    }
}
