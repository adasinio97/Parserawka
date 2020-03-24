using ParserawkaWPF.Interfaces;

namespace ParserawkaWPF.Model
{
    public class Statement
    {
        public int ProgramLine { get; set; }
        public IStatementList StatementList { get; set; }
        public Statement(int a)
        {
            ProgramLine = a;
        }
    }
}
