using ParserawkaWPF.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class Assign : Statement
    {
        public Variable LeftSide { get; set; }
        public Expression RightSide { get; set; }

        public Assign(int a) : base(a) { }
        public Assign(AstAssign ast) : base(ast) { }
    }
}
