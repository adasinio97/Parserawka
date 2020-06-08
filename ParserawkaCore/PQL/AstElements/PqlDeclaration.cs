using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlDeclaration : PqlAst, IEntity
    {
        public PqlToken DesignEntity { get; set; }
        public PqlSynonym Synonym { get; set; }
        public System.Type DeclarationType { get; set; }

        public string AttributeValue => Synonym.Name;

        public bool IsSecondaryAttribute { get; set; }

        public EntityList EntityList { get; set; }

        public Attribute Attribute { get; set; }
        public Attribute SecondaryAttribute { get; set; }

        public PqlDeclaration(PqlToken designEntity, PqlSynonym synonym)
        {
            DesignEntity = designEntity;
            Synonym = synonym;
            IsSecondaryAttribute = false;
            
            switch (DesignEntity.Type)
            {
                case PqlTokenType.PROCEDURE:
                    DeclarationType = typeof(Procedure);
                    break;
                case PqlTokenType.PROG_LINE:
                case PqlTokenType.STMT:
                    DeclarationType = typeof(Statement);
                    break;
                case PqlTokenType.ASSIGN:
                    DeclarationType = typeof(Assign);
                    break;
                case PqlTokenType.CALL:
                    DeclarationType = typeof(Call);
                    break;
                case PqlTokenType.WHILE:
                    DeclarationType = typeof(While);
                    break;
                case PqlTokenType.IF:
                    DeclarationType = typeof(If);
                    break;
                case PqlTokenType.VARIABLE:
                    DeclarationType = typeof(Variable);
                    break;
                case PqlTokenType.CONSTANT:
                    DeclarationType = typeof(Constant);
                    break;
            }

            Attribute = new Attribute("synonym", Synonym.Name);
        }
    }
}
