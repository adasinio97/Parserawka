namespace ParserawkaWPF.Model
{
    public class Modify
    {
        public Statement Statement { get; set; }
        public Variable Variable { get; set; }
        public Modify(Statement a, Variable v)
        {
            this.Statement = a;
            this.Variable = v;
        }
    }
}
