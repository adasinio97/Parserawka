using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;

namespace ParserawkaWPF.Model
{
    public class Variable : Factor
    {
        public string Name { get; set; }
        public IVariableList VariableList { get; set; }
        public Variable(string name)
        {
            Name = name;
        }

        public Variable(AstVariable ast)
        {
            this.Name = ast.name;
        }
    }
}
