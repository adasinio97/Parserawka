using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser;
using ParserawkaCore.Parser.AstElements;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    class ProgramKnowledgeBase : IProgramKnowledgeBase
    {
        private static ProgramKnowledgeBase instance;

        private ProgramKnowledgeBase() { }

        public static ProgramKnowledgeBase GetInstance()
        {
            if (instance == null)
                instance = new ProgramKnowledgeBase();
            return instance;
        }

        public IVariableList Variables { get; set; }

        public IStatementList Statements { get; set; }

        public IFollowsTable FollowsTable { get; set; }

        public IParentTable ParentTable { get; set; }

        public IModifiesTable ModifiesTable { get; set; }

        public IUsesTable UsesTable { get; set; }

        public void LoadData(string programCode)
        {
            Lexer lexer = new Lexer(programCode);
            IParser parser = new Parser.Parser(lexer);
            AST root = parser.Parse();
            IDesignExtractor designExtractor = ImplementationFactory.CreateDesignExtractor();
            designExtractor.ExtractData(root);

            Variables = designExtractor.Variables;
            Statements = designExtractor.Statements;
            FollowsTable = designExtractor.FollowsTable;
            ParentTable = designExtractor.ParentTable;
            ModifiesTable = designExtractor.ModifiesTable;
            UsesTable = designExtractor.UsesTable;
        }
    }
}
