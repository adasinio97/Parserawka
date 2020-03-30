namespace ParserawkaWPF.Model
{
    public class Follows
    {
        public Statement FirstStatement { get; set; }
        public Statement SecondStatement { get; set; }

        public Follows(Statement s1, Statement s2)
        {
            this.FirstStatement = s1;
            this.SecondStatement = s2;
        }
    }
}
