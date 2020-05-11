using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Interfaces
{
    public interface IDesignExtractor
    {
        IVariableList Variables { get; }
        IStatementList Statements { get; }
        IProcedureList Procedures { get; }
        IConstantList Constants { get; }
        IFollowsTable FollowsTable { get; }
        IParentTable ParentTable { get; }
        IModifiesTable ModifiesTable { get; }
        IUsesTable UsesTable { get; }
        ICallsTable CallsTable { get; }
        INextTable NextTable { get; }
        IAffectsTable AffectsTable { get; }
        void ExtractData(AST root);
    }
}
