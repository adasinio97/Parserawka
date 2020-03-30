using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Utils;
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

        public IStatementList GetFollowedByT(Statement statement, IStatementList statementList)
        {
            Statement tmp = GetFollowedBy(statement);
            if (tmp != null)
            {
                GetFollowedByT(tmp, statementList);
                statementList.AddStatement(tmp);
            }
            return statementList;
        }

        public Statement GetFollows(Statement statement)
        {
            return FollowsList.Where(x => x.FirstStatement == statement).FirstOrDefault().SecondStatement;
        }

        public IStatementList GetFollowsT(Statement statement,IStatementList statementList)
        {
            Statement tmp = GetFollowedBy(statement);
            if(tmp != null)
            {
                GetFollowedByT(tmp, statementList);
                statementList.AddStatement(tmp);
            }
            return statementList;

        }

        public bool IsFollows(Statement firstStatement, Statement secondStatement)
        {
           return firstStatement == GetFollowedBy(secondStatement);
        }

        public bool IsFollowsT(Statement firstStatement, Statement secondStatement)
        {
            IStatementList statementList = ImplementationFactory.CreateStatementList();
            GetFollowedByT(secondStatement,statementList);
            for(int i = 0; i < statementList.GetSize(); i++)
            {
                if (statementList.GetStatementByIndex(i) == firstStatement)
                    return true;
            }
            return false;

        }

        public void SetFollows(Statement firstStatement, Statement secondStatement)
        {
            FollowsList.Add(new Follows(firstStatement, secondStatement));
        }
    }
}
