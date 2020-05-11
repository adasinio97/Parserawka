using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class Affects
    {
        public Assign FirstAssignment { get; set; }
        public Assign SecondAssignment { get; set; }

        public Affects(Assign firstAssignment, Assign secondAssignment)
        {
            FirstAssignment = firstAssignment;
            SecondAssignment = secondAssignment;
        }
    }
}
