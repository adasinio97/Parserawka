using ParserawkaWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaWPF.Model
{
    class FollowsTable : IFollowsTable
    {
        List<Follows> FollowsList = new List<Follows>();
        public Statement GetFollowedBy(Statement statement)
        {
            return FollowsList.Where(x => x.SecondStatement == statement).FirstOrDefault().FirstStatement;
        }

        public IStatementList GetFollowedByT(Statement statement)
        {
            throw new NotImplementedException();
        }

        public Statement GetFollows(Statement statement)
        {
            return FollowsList.Where(x => x.FirstStatement == statement).FirstOrDefault().SecondStatement;
        }

        public IStatementList GetFollowsT(Statement statement)
        {
            throw new NotImplementedException();
        }

        public bool IsFollows(Statement firstStatement, Statement secondStatement)
        {
            throw new NotImplementedException();
        }

        public bool IsFollowsT(Statement firstStatement, Statement secondStatement)
        {
            throw new NotImplementedException();
        }

        public void SetFollows(Statement firstStatement, Statement secondStatement)
        {
            FollowsList.Add(new Follows(firstStatement, secondStatement));
        }
    }
}
