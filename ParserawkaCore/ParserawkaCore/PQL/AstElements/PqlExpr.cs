﻿using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlExpr : PqlAst
    {
        AST ExprTree { get; set; }
    }
}
