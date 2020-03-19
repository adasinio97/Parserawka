using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB
{
    interface IModifiesTable
    {
        void setModifies(Statement statement, Variable variable);
        IStatementList getModifies(Variable variable);
        IVariableList getModifiedBy(Statement statement);
        bool isModified(Statement statement, Variable variable);
    }
}
