﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlModifies : PqlRelation
    {		
		public PqlModifies(PqlAst leftRef, PqlAst rightRef) : base(leftRef, rightRef) { }
    }
}
