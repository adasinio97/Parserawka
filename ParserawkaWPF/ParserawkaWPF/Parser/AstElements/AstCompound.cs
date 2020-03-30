using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstCompound : AST
    {
        public List<AST> children;

        public AstCompound()
        {
            children = new List<AST>();
        }
    }
}
