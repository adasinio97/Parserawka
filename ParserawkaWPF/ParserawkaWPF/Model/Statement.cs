using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
using ParserawkaWPF.Utils;

namespace ParserawkaWPF.Model
{
    public class Statement
    {
        public int ProgramLine { get; set; }
        public IStatementList StatementList { get; set; }
        public Statement(int a)
        {
            ProgramLine = a;
            StatementList = ImplementationFactory.CreateStatementList();
        }
        public Statement(AstStatement ast)
        {
            ProgramLine = ast.ProgramLine;
            StatementList = ImplementationFactory.CreateStatementList();
        }
    }
}
