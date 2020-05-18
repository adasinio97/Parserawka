﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlPatternNode : PqlClause
    {
        public PqlSynonym Synonym { get; set; }
        public PqlAst VarRef { get; set; }
        public PqlAst Expr { get; set; } //Jeśli pattern jest typu if albo while to będzie tu null

        public PqlPatternNode(PqlSynonym syn, PqlAst varRef,PqlAst expr)
        {
            Synonym = syn;
            VarRef = varRef;
            Expr = expr;
        }
    }
}
