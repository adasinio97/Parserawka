using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class Variable : Factor, IEntity
    {
        public string Name { get; set; }
        
        public Token Token { get; set; }

        public string AttributeValue => Name;

        public Attribute Attribute { get; set; }
        public Attribute SecondaryAttribute { get; set; }

        public IProcedureList ModifiedByProcedures { get; set; }
        public IStatementList ModifiedByStatements { get; set; }

        public IProcedureList UsedByProcedures { get; set; }
        public IStatementList UsedByStatements { get; set; }

        public Variable(string name)
        {
            Name = name;
            Attribute = new Attribute("varName", Name);
            ModifiedByProcedures = ImplementationFactory.CreateProcedureList();
            ModifiedByStatements = ImplementationFactory.CreateStatementList();
            UsedByProcedures = ImplementationFactory.CreateProcedureList();
            UsedByStatements = ImplementationFactory.CreateStatementList();
        }

        public Variable(Token token)
        {
            Name = token.Value.ToString();
            Attribute = new Attribute("varName", Name);
            ModifiedByProcedures = ImplementationFactory.CreateProcedureList();
            ModifiedByStatements = ImplementationFactory.CreateStatementList();
            UsedByProcedures = ImplementationFactory.CreateProcedureList();
            UsedByStatements = ImplementationFactory.CreateStatementList();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
