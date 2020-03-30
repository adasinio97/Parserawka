using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Interfaces
{
    /* Do wyrzucenia, jak już będzie AST. */
    public interface ITmpAst { }

    public interface IDesignExtractor
    {
        IVariableList Variables { get; }
        IStatementList Statements { get; }
        IFollowsTable FollowsTable { get; }
        IParentTable ParentTable { get; }
        IModifiesTable ModifiesTable { get; }
        IUsesTable UsesTable { get; }
        void ExtractData(ITmpAst abstractSyntaxTree);
    }
}
