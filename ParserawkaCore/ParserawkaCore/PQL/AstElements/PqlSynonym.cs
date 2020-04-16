using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlSynonym : PqlArgument, PqlElem
    {
        public PqlToken Ident;
        public PqlSynonym(PqlToken ident)
        {
            Ident = ident;
        }
    }
}
