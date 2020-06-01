﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Model
{
    public abstract class Container : Statement
    {
        public Variable Condition { get; set; }

        public Container(int programLine) : base(programLine) { }
    }
}
