using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
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
