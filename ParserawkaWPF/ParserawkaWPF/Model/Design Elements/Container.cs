﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public abstract class Container : Statement
    {
        public Container(int programLine) : base(programLine) { }
    }
}
