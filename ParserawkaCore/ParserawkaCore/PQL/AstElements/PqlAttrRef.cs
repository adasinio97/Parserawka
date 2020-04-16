using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlAttrRef : PqlElem
    {
        public PqlToken Synonym;
        public PqlToken AttrName;
        public PqlAttrRef(PqlToken synonym, PqlToken attrName)
        {
            Synonym = synonym;
            AttrName = attrName;
        }
    }
}
