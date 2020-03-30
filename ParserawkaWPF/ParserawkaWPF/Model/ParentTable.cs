using System.Collections.Generic;
using System.Linq;
using ParserawkaWPF.Interfaces;

namespace ParserawkaWPF.Model
{
    class ParentTable : IParentTable
    {
        List<Parent> ParentList = new List<Parent>();
        public Statement GetParent(Statement statement)
        {
            return ParentList.Where(x => x.ChildStatement == statement).FirstOrDefault()?.ParentStatement;
        }

        public IStatementList GetParentedBy(Statement statement)
        {
            return ParentList.Where(x => x.ParentStatement == statement).FirstOrDefault()?.ParentStatement.StatementList;
        }
    

        public IStatementList GetParentedByT(Statement statement, IStatementList statementList) // parent*, przed wywołaniem metody należy utworzyć statementList
        {
            var tmpStatementList = GetParentedBy(statement);
            foreach (var item in GetParentedBy(statement))
            {
                GetParentedByT(item, statementList);
                statementList.AddStatement(item);
            }
            return statementList;
        }

        public IStatementList GetParentT(Statement statement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            var parent = GetParent(statement);
            if (parent != null)
            {
                GetParentT(parent, statementList);
                statementList.AddStatement(parent);
            }
            return statementList;
        }

        public bool IsParent(Statement firstStatement, Statement secondStatement)
        {
            return GetParent(secondStatement) == firstStatement;
        }

        public bool IsParentT(Statement firstStatement, Statement secondStatement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            return GetParentT(secondStatement, statementList).Any(x => x == firstStatement); //lista pośrednich parentów
        }

        public void SetParent(Statement firstStatement, Statement secondStatement)
        {
            ParentList.Add(new Parent(firstStatement, secondStatement));
        }
    }
}
