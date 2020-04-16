using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL.AstElements
{
    public class PqlDeclaration : PqlAst
    {
        PqlToken DesignEntity;
        PqlSynonym Synonym;

        public PqlDeclaration(PqlToken designEntity, PqlSynonym synonym)
        {
            DesignEntity = designEntity;
            Synonym = synonym;
        }

    }
}
