using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF
{
    class Statement
    {
        public int programLine { get; set; }
        public IStatementList statementList { get; set; }
        public Statement(int a)
        {

        }
    }
}
