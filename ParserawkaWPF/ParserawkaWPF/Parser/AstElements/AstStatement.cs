using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public abstract class AstStatement : AST
    {
        public int ProgramLine { get; set; }
        public AstStatement(int line) 
        {
            this.ProgramLine = line;
        }
    }
}
