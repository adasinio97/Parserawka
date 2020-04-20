using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlSynonym : PqlArgument, PqlElem
    {
        public PqlToken Ident { get; set; }
        public string Name { get; set; }

        public PqlSynonym(PqlToken ident)
        {
            Ident = ident;
            Name = ident.Value.ToString();
        }
    }
}
