using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;
using ParserawkaCore.Parser.AstElements;

namespace ParserawkaCore.Model
{
    public class Variable : Factor
    {
        public string Name { get; set; }
        
        public Token Token { get; set; }

        public Variable(string name)
        {
            Name = name;
        }

        public Variable(Token token)
        {
            Name = token.Value.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
