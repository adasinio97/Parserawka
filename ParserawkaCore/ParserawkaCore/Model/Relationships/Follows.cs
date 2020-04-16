namespace ParserawkaCore.Model
{
    public class Follows
    {
        public Statement FirstStatement { get; set; }
        public Statement SecondStatement { get; set; }

        public Follows(Statement firstStatement, Statement secondStatement)
        {
            FirstStatement = firstStatement;
            SecondStatement = secondStatement;
        }
    }
}
