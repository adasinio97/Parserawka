using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB
{
    interface IFollowsTable
    {
        void setFollows(Statement firstStatement, Statement secondStatement);
        Statement getFollows(Statement statement);
        IStatementList getFollowsT(Statement statement);
        Statement getFollowedBy(Statement statement);
        IStatementList getFollowedByT(Statement statement);
        bool isFollows(Statement firstStatement, Statement secondStatement);
        bool isFollowsT(Statement firstStatement, Statement secondStatement);
    }
}
