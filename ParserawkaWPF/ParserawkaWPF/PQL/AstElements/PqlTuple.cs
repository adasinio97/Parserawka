using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlTuple : PqlAst
    {
        public List<PqlAst> Elems;

        public PqlTuple(params PqlAst[] elems)
        {
            Elems = new List<PqlAst>();
            Elems.AddRange(elems);
        }
    }
}
