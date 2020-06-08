using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.Model
{
    public class PqlBooleanOutput : PqlOutput
    {
        public bool Value { get; set; }

        public PqlBooleanOutput(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }
    }
}
