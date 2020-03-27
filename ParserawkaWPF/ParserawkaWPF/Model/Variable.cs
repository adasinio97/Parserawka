using ParserawkaWPF.Interfaces;

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
    }
}
