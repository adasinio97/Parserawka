﻿using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public class If : Container
    {
        public IStatementList IfBody { get; set; }
        public IStatementList ElseBody { get; set; }

        public If (Variable condition, IStatementList ifBody, IStatementList elseBody, int programLine) : base(programLine)
        {
            Condition = condition;
            IfBody = ifBody;
            ElseBody = elseBody;
        }

        public override string ToString()
        {
            return base.ToString() + " if " + Condition.ToString();
        }
    }
}
