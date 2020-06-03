using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlInteger : PqlArgument
    {
        public string Value { get; set; }

        public PqlInteger(PqlToken token)
        {
            Value = token.Value.ToString();
        }
    }
}
