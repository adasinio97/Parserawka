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

        public IVariableList Modifying { get; set; }
        public IVariableList Using { get; set; }

        public IProcedureList CalledBy { get; set; }
        public IProcedureList Calling { get; set; }

        public Procedure(string name)
        {
            Name = name;
            Body = ImplementationFactory.CreateStatementList();
            Attribute = new Attribute("procName", Name);
            Modifying = ImplementationFactory.CreateVariableList();
            Using = ImplementationFactory.CreateVariableList();
            CalledBy = ImplementationFactory.CreateProcedureList();
            Calling = ImplementationFactory.CreateProcedureList();
        }

        public Procedure(string name, IStatementList body)
        {
            Name = name;
            Body = body;
            Attribute = new Attribute("procName", Name);
            Modifying = ImplementationFactory.CreateVariableList();
            Using = ImplementationFactory.CreateVariableList();
            CalledBy = ImplementationFactory.CreateProcedureList();
            Calling = ImplementationFactory.CreateProcedureList();
        }

        public override string ToString()
        {
            return "procedure " + Name;
        }
    }
}
