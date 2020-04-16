using System.Collections.Generic;
using System.Linq;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    class ParentTable : IParentTable
    {
        private List<Parent> parentList = new List<Parent>();

        public Statement GetParent(Statement statement)
        {
            return parentList.Where(x => x.ChildStatement == statement).FirstOrDefault()?.ParentStatement;
        }

        public IStatementList GetParentedBy(Statement statement)
        {
            List<Parent> list = parentList.Where(x => x.ParentStatement == statement).ToList();
            IStatementList statements = ImplementationFactory.CreateStatementList();

            foreach (Parent parentPair in list)
            {
                statements.AddStatement(parentPair.ChildStatement);
            }
            return statements;
        }

        public IStatementList GetParentedByT(Statement statement)
        {
            return new RecursionContext(this).GetParentedByT(statement);
        }

        public IStatementList GetParentT(Statement statement)
        {
            return new RecursionContext(this).GetParentT(statement);
        }

        public bool IsParent(Statement firstStatement, Statement secondStatement)
        {
            return GetParent(secondStatement) == firstStatement;
        }

        public bool IsParentT(Statement firstStatement, Statement secondStatement)
        {
            IStatementList parents = GetParentT(secondStatement);
            return parents.Contains(firstStatement);
        }

        public void SetParent(Statement firstStatement, Statement secondStatement)
        {
            parentList.Add(new Parent(firstStatement, secondStatement));
        }

        private class RecursionContext
        {
            private ParentTable outer;
            private IStatementList statementList;

            public RecursionContext(ParentTable outer)
            {
                statementList = ImplementationFactory.CreateStatementList();
                this.outer = outer;
            }

            public IStatementList GetParentedByT(Statement statement)
            {
                IStatementList parentedList = outer.GetParentedBy(statement);
                if (parentedList != null)
                {
                    foreach (Statement parentedStatement in parentedList)
                    {
                        GetParentedByT(parentedStatement);
                        statementList.AddStatement(parentedStatement);
                    }
                }
                return statementList;
            }

            public IStatementList GetParentT(Statement statement)
            {
                Statement parent = outer.GetParent(statement);
                if (parent != null)
                {
                    GetParentT(parent);
                    statementList.AddStatement(parent);
                }
                return statementList;
            }
        }
    }
}
