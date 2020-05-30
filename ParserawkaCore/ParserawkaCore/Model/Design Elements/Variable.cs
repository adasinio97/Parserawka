using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;

namespace ParserawkaCore.Model
{
    public class Variable : Factor, IEntity
    {
        public string Name { get; set; }
        
        public Token Token { get; set; }

        public string AttributeValue => Name;

        public Attribute Attribute { get; set; }
        public Attribute SecondaryAttribute { get; set; }

        public Variable(string name)
        {
            Name = name;
            Attribute = new Attribute("varName", Name);
        }

        public Variable(Token token)
        {
            Name = token.Value.ToString();
            Attribute = new Attribute("varName", Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
