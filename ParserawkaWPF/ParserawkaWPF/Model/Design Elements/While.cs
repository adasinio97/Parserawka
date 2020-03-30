using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class While : Statement
    {
        public While(int a) : base(a) { }
        public While(AstWhileStatement ast) : base(ast) { }
    }
}
