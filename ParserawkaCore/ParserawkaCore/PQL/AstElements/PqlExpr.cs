using ParserawkaCore.Model;
using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlExpr : PqlAst
    {
        public Factor ExprTree { get; set; }
        public bool IsExact { get; set; }

        public PqlExpr(bool isExact)
        {
            IsExact = isExact;
        }
    }
}
