using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB
{
    interface IUsesTable
    {
        void setUses(Statement statement, Variable variable);
        IStatementList getUses(Variable variable);
        IVariableList getUsedBy(Statement statement);
        bool IsUses(Statement statement, Variable variable);
    }
}
