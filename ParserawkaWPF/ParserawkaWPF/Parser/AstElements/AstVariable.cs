using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstVariable : AST
    {
        public Token token { get; set; }
        public string name { get; set; }

        public AstVariable(Token token)
        {
            this.token = token;
            name = token.Value.ToString();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
