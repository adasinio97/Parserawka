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

        public string SynonymName;
        public string AttributeName;

        public PqlAttrRef(PqlToken synonym, PqlToken attrName)
        {
            Synonym = synonym;
            AttrName = attrName;

            SynonymName = Synonym.Value.ToString();
            AttributeName = AttrName.Value.ToString();
        }

        public PqlAttrRef(PqlToken synonym)
        {
            Synonym = synonym;
            AttributeName = "progLine";
            SynonymName = Synonym.Value.ToString();
        }
    }
}
