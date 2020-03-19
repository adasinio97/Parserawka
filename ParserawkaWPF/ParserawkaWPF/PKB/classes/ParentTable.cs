using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB.classes
{
    class ParentTable : IParentTable
    {
        List<Parent> parentList = new List<Parent>();
        public Statement getParent(Statement statement)
        {
            return parentList.Where(x => x.child == statement).FirstOrDefault().parent;
        }

        public IStatementList getParentedBy(Statement statement)
        {
            return parentList.Where(x => x.parent == statement).FirstOrDefault().child.statementList;
        }
    

        public IStatementList getParentedByT(Statement statement, IStatementList statementList) // parent*, przed wywołaniem metody należy utworzyć statementList
        {
            IStatementList tmpStatementList = getParentedBy(statement);
            for (int i = 0; i < tmpStatementList.getSize(); i++)
            {
                getParentedByT(tmpStatementList.getStatementByIndex(i), statementList);
                statementList.addStatement(tmpStatementList.getStatementByIndex(i));
            }

            return statementList;

        }

        public IStatementList getParentT(Statement statement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            Statement parent = getParent(statement);
            if (parent != null)
            {
                getParentT(parent, statementList);
                statementList.addStatement(parent);
            }
            return statementList;
        }

        public bool isParent(Statement firstStatement, Statement secondStatement)
        {
            Statement isParent = getParent(secondStatement);
            return firstStatement == isParent;
        }

        public bool isParentT(Statement firstStatement, Statement secondStatement, IStatementList statementList) // przed wywołaniem metody należy utworzyć statementList
        {
            IStatementList listaParentowPosrednich = getParentT(secondStatement, statementList);
            for(int i = 0; i < listaParentowPosrednich.getSize(); i++)
            {
                if (listaParentowPosrednich.getStatementByIndex(i) == firstStatement)
                    return true;
            }
            return false;
        }

        public void setParent(Statement firstStatement, Statement secondStatement)
        {
            parentList.Add(new Parent(firstStatement, secondStatement));
        }
    }
}
