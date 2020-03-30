using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    public class AstProcedure : AST
    {
        public string testName { get; set; }

        public AST Body { get; set; }

        public AstProcedure(string name, AST body)
        {
            testName = name;
            Body = body;
        }

        public override string ToString()
        {
            return testName;
        }
    }
}
