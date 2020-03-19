using ParserawkaWPF.Interfaces;

namespace ParserawkaWPF.Model
{
    class Statement
    {
        public int ProgramLine { get; set; }
        public IStatementList StatementList { get; set; }
        public Statement(int a)
        {

        }
    }
}
