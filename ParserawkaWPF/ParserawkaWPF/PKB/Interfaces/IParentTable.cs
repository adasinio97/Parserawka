using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB
{
    interface IParentTable
    {
         void setParent(Statement firstStatement, Statement secondStatement);

        Statement getParent(Statement statement);
        IStatementList getParentT(Statement statement, IStatementList statementList);
        IStatementList getParentedBy(Statement statement);
        IStatementList getParentedByT(Statement statement, IStatementList statementList);
        bool isParent(Statement firstStatement, Statement secondStatement);
        bool isParentT(Statement firstStatement, Statement secondStatement, IStatementList statementList);
    }
}
