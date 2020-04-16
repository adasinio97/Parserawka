namespace ParserawkaCore.Model
{
    public class Uses
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }

        public Uses(Statement statement, Variable variable)
        {
            Statement = statement;
            Variable = variable;
        }
    }
}
