namespace ParserawkaCore.Model
{
    public class Modify
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }

        public Modify(Statement statement, Variable variable)
        {
            Statement = statement;
            Variable = variable;
        }
    }
}
