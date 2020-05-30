﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlPattern : PqlClause
    {
        public List<PqlPatternCond> PatternCond;

        public PqlPattern(List<PqlPatternCond> patternCond)
        {
            PatternCond = patternCond;
        }
    }
}