using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class Statement : IEntity
    {
        public int ProgramLine { get; set; }

        public Attribute Attribute { get; set; }

        public Statement(int programLine)
        {
            ProgramLine = programLine;
            Attribute = new Attribute("progLine", ProgramLine.ToString());
        }

        public override string ToString()
        {
            return ProgramLine.ToString();
        }
    }
}
