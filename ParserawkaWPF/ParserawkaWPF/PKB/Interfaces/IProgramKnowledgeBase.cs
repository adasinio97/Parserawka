using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB
{
    interface IProgramKnowledgeBase
    {
        //  public IAbstractSyntaxTree AbstractSyntaxTree { get; }
        IVariableList variables { get; }
        IStatementList statements { get; }
        IFollowsTable followsTable { get; }
        IParentTable parentTable { get; }
        IModifiesTable modifiesTable { get; }
        IUsesTable usesTable { get; }
        void LoadData();

    }
}
