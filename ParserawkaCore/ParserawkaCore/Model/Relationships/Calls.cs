using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class Calls
    {
        public Procedure FirstProcedure { get; set; }
        public Procedure SecondProcedure { get; set; }

        public Calls(Procedure firstProcedure, Procedure secondProcedure)
        {
            FirstProcedure = firstProcedure;
            SecondProcedure = secondProcedure;
        }
    }
}
