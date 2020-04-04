using ParserawkaWPF.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class Constant : Factor
    {
        public int Value { get; set; }

        public Token Token { get; set; }

        public Constant(Token token)
        {
            Token = token;
            Value = int.Parse(token.Value.ToString());
        }

        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }
}
