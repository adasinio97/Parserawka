namespace ParserawkaWPF.Interfaces
{
    public interface IProgramKnowledgeBase
    {
        //  public IAbstractSyntaxTree AbstractSyntaxTree { get; }
        IVariableList Variables { get; }
        IStatementList Statements { get; }
        IFollowsTable FollowsTable { get; }
        IParentTable ParentTable { get; }
        IModifiesTable ModifiesTable { get; }
        IUsesTable UsesTable { get; }
        void LoadData();
    }
}
