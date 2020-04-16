using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class Procedure : AST
    {
        public string Name { get; set; }

        public IStatementList Body { get; set; }

        public Procedure(string name)
        {
            Name = name;
            Body = ImplementationFactory.CreateStatementList();
        }

        public Procedure(string name, IStatementList body)
        {
            Name = name;
            Body = body;
        }

        public override string ToString()
        {
            return "procedure " + Name;
        }
    }
}
