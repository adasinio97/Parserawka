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
            return ParentList.Where(x => x.ChildStatement == statement).FirstOrDefault().ParentStatement;
        }

        public IStatementList GetParentedBy(Statement statement)
        {
            return ParentList.Where(x => x.ParentStatement == statement).FirstOrDefault().ParentStatement.StatementList;
        }
    

        public IStatementList GetParentedByT(Statement statement, IStatementList statementList) // parent*, przed wywołaniem metody należy utworzyć statementList
        {
            IStatementList tmpStatementList = GetParentedBy(statement);
            for (int i = 0; i < tmpStatementList.GetSize(); i++)
            {
                GetParentedByT(tmpStatementList.GetStatementByIndex(i), statementList);
                statementList.AddStatement(tmpStatementList.GetStatementByIndex(i));
            }

            return statementList;

        }

        public IStatementList GetParentT(Statement statement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            Statement parent = GetParent(statement);
            if (parent != null)
            {
                GetParentT(parent, statementList);
                statementList.AddStatement(parent);
            }
            return statementList;
        }

        public bool IsParent(Statement firstStatement, Statement secondStatement)
        {
            Statement isParent = GetParent(secondStatement);
            return firstStatement == isParent;
        }

        public bool IsParentT(Statement firstStatement, Statement secondStatement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            IStatementList indirectParentList = GetParentT(secondStatement, statementList); //lista pośrednich parentów
            for(int i = 0; i < indirectParentList.GetSize(); i++)
            {
                if (indirectParentList.GetStatementByIndex(i) == firstStatement)
                    return true;
            }
            return false;
        }

        public void SetParent(Statement firstStatement, Statement secondStatement)
        {
            ParentList.Add(new Parent(firstStatement, secondStatement));
        }
    }
}
