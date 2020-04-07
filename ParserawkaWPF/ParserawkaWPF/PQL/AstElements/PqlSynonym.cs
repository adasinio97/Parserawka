using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlSynonym : PqlAst
    {
        public PqlToken Ident;
        public PqlSynonym(PqlToken ident)
        {
            Ident = ident;
        }
    }
}
