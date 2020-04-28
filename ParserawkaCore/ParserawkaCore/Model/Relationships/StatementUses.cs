namespace ParserawkaCore.Model
{
    public class StatementUses
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }

        public StatementUses(Statement statement, Variable variable)
        {
            Statement = statement;
            Variable = variable;
        }
    }
}
