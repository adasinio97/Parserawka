using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class Constant : Factor, IEntity
    {
        public int Value { get; set; }

        public Token Token { get; set; }

        public Attribute Attribute { get; set; }
        public Attribute SecondaryAttribute { get; set; }

        public Constant(Token token)
        {
            Token = token;
            Value = int.Parse(token.Value.ToString());
            Attribute = new Attribute("value", token.Value.ToString());
        }

        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }
}
