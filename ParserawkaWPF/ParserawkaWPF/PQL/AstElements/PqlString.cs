using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlString : PqlArgument
    {
        public string Value { get; set; }

        public PqlString(PqlToken token)
        {
            Value = token.Value.ToString();
        }
    }
}
