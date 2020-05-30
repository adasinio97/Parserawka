using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class Procedure : AST, IEntity
    {
        public string Name { get; set; }

        public IStatementList Body { get; set; }

        public Attribute Attribute { get; set; }
        public Attribute SecondaryAttribute { get; set; }

        public Procedure(string name)
        {
            Name = name;
            Body = ImplementationFactory.CreateStatementList();
            Attribute = new Attribute("procName", Name);
        }

        public Procedure(string name, IStatementList body)
        {
            Name = name;
            Body = body;
            Attribute = new Attribute("procName", Name);
        }

        public override string ToString()
        {
            return "procedure " + Name;
        }
    }
}
