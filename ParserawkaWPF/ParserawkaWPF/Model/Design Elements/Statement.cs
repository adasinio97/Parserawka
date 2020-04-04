using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
using ParserawkaWPF.Utils;

namespace ParserawkaWPF.Model
{
    public class Statement
    {
        public int ProgramLine { get; set; }

        public Statement(int programLine)
        {
            ProgramLine = programLine;
        }

        public override string ToString()
        {
            return ProgramLine.ToString();
        }
    }
}
