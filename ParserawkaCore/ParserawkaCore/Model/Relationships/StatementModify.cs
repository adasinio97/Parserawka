namespace ParserawkaCore.Model
{
    public class StatementModify
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }

        public StatementModify(Statement statement, Variable variable)
        {
            Statement = statement;
            Variable = variable;
        }
    }
}
