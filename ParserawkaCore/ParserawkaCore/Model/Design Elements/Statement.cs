using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class Statement : IEntity
    {
        public int ProgramLine { get; set; }

        public Attribute Attribute { get; set; }
        public virtual Attribute SecondaryAttribute { get; set; }

        public Statement FollowedBy { get; set; }
        public Statement Following { get; set; }

        public IVariableList Modifying { get; set; }
        public IVariableList Using { get; set; }

        public IStatementList NextedBy { get; set; }
        public IStatementList Nexting { get; set; }

        public Statement Parent { get; set; }
        public IStatementList Children { get; set; }

        public Statement(int programLine)
        {
            ProgramLine = programLine;
            Attribute = new Attribute("progLine", ProgramLine.ToString());
            Modifying = ImplementationFactory.CreateVariableList();
            Using = ImplementationFactory.CreateVariableList();
            NextedBy = ImplementationFactory.CreateStatementList();
            Nexting = ImplementationFactory.CreateStatementList();
            Children = ImplementationFactory.CreateStatementList();
        }

        public override string ToString()
        {
            return ProgramLine.ToString();
        }
    }
}
