using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstDeclaration : AST
    {
        public Token type;
        public Token id;
        public AST value;

        public AstDeclaration(Token type, Token id, AST value)
        {
            this.type = type;
            this.id = id;
            this.value = value;
        }

        public override string ToString()
        {
            return type.Value.ToString() + id.Value.ToString() + " = " + value.ToString();
        }
    }
}
