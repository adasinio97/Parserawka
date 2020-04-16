using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaCore.Model
{
    class FollowsTable : IFollowsTable
    {
        private List<Follows> followsList = new List<Follows>();

        public Statement GetFollowedBy(Statement statement)
        {
            return followsList.Where(x => x.SecondStatement == statement).FirstOrDefault().FirstStatement;
        }

        public IStatementList GetFollowedByT(Statement statement)
        {
            return new RecursionContext(this).GetFollowedByT(statement);
        }

        public Statement GetFollows(Statement statement)
        {
            return followsList.Where(x => x.FirstStatement == statement).FirstOrDefault().SecondStatement;
        }

        public IStatementList GetFollowsT(Statement statement)
        {
            return new RecursionContext(this).GetFollowsT(statement);
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
            followsList.Add(new Follows(firstStatement, secondStatement));
        }

        /* Klasa do rekursji - przetrzymuje aktualną listę */
        private class RecursionContext
        {
            private IStatementList statementList;
            private FollowsTable outer;

            public RecursionContext(FollowsTable outer)
            {
                statementList = ImplementationFactory.CreateStatementList();
                this.outer = outer;
            }

            public IStatementList GetFollowedByT(Statement statement)
            {
                Statement followedStatement = outer.GetFollowedBy(statement);
                if (followedStatement != null)
                {
                    GetFollowedByT(followedStatement);
                    statementList.AddStatement(followedStatement);
                }
                return statementList;
            }

            public IStatementList GetFollowsT(Statement statement)
            {
                Statement followingStatement = outer.GetFollows(statement);
                if (followingStatement != null)
                {
                    GetFollowsT(followingStatement);
                    statementList.AddStatement(followingStatement);
                }
                return statementList;
            }
        }
    }
}
