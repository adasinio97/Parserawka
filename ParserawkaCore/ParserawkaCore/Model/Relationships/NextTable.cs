using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.Model
{
    public class NextTable : INextTable
    {
        private List<Next> nextList = new List<Next>();

        public IStatementList GetNext(Statement statement)
        {
            List<Next> list = nextList.Where(x => x.FirstStatement == statement).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            foreach (Next next in list)
                statementList.AddStatement(next.SecondStatement);

            return statementList;
        }

        public IStatementList GetNextedBy(Statement statement)
        {
            List<Next> list = nextList.Where(x => x.SecondStatement == statement).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            foreach (Next next in list)
                statementList.AddStatement(next.FirstStatement);

            return statementList;
        }

        public IStatementList GetNextedByT(Statement statement)
        {
            return new RecursionContext(this).GetNextedByT(statement);
        }

        public IStatementList GetNextT(Statement statement)
        {
            return new RecursionContext(this).GetNextT(statement);
        }

        public bool IsNext(Statement statement1, Statement statement2)
        {
            IStatementList statementList = GetNext(statement1);
            return statementList.Contains(statement2);
        }

        public bool IsNextT(Statement statement1, Statement statement2)
        {
            IStatementList statementList = GetNextT(statement1);
            return statementList.Contains(statement2);
        }

        public void SetNext(Statement statement1, Statement statement2)
        {
            if (!IsNext(statement1, statement2))
                nextList.Add(new Next(statement1, statement2));
        }

        private class RecursionContext
        {
            private IStatementList statementList;
            private NextTable outer;

            public RecursionContext(NextTable outer)
            {
                statementList = ImplementationFactory.CreateStatementList();
                this.outer = outer;
            }

            public IStatementList GetNextT(Statement statement)
            {
                IStatementList nextList = outer.GetNext(statement);
                if (nextList != null)
                {
                    foreach (Statement nextStatement in nextList)
                    {
                        if (statementList.Contains(nextStatement))
                            continue;
                        else
                        {
                            statementList.AddStatement(nextStatement);
                            GetNextT(nextStatement);
                        }
                    }
                }
                return statementList;
            }

            public IStatementList GetNextedByT(Statement statement)
            {
                IStatementList nextedList = outer.GetNextedBy(statement);
                if (nextedList != null)
                {
                    foreach (Statement nextedStatement in nextedList)
                    {
                        if (statementList.Contains(nextedStatement))
                            continue;
                        else
                        {
                            statementList.AddStatement(nextedStatement);
                            GetNextedByT(nextedStatement);
                        }
                    }
                }
                return statementList;
            }
        }
    }
}
