using ParserawkaCore.Parser.AstElements;

namespace ParserawkaCore.Interfaces
{
    public interface IProgramKnowledgeBase
    {
        AST AbstractSyntaxTree { get; }
        IVariableList Variables { get; }
        IStatementList Statements { get; }
        IProcedureList Procedures { get; }
        IConstantList Constants { get; }
        IFollowsTable FollowsTable { get; }
        IParentTable ParentTable { get; }
        IModifiesTable ModifiesTable { get; }
        IUsesTable UsesTable { get; }
        ICallsTable CallsTable { get; }
        void LoadData(string programCode);
    }
}
