using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstNum : AST
    {
        public Token token;

        public AstNum(Token token)
        {
            this.token = token;
        }

        public override string ToString()
        {
            return token.Value.ToString();
        }
    }
}
