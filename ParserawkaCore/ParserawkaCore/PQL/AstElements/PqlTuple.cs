using ParserawkaCore.PQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlTuple : PqlResult
    {
        public List<PqlElem> Elems;

        public PqlTuple(params PqlElem[] elems)
        {
            Elems = new List<PqlElem>();
            Elems.AddRange(elems);
        }
    }
}
