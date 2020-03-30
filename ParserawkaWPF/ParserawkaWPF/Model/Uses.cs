namespace ParserawkaWPF.Model
{
    public class Uses
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }

        public Uses()
        {

        }
        public Uses(Statement statement, Variable variable)
        {
            this.Statement = statement;
            this.Variable = variable;
        }
    }
}
