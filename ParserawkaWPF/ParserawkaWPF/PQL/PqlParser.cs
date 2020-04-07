using ParserawkaWPF.PQL.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PQL
{
    public class PqlParser
    {
        public PqlLexer lexer { get; set; }

        private int statementCounter;
        private bool isTestFile;

        private PqlToken currentToken;

        public PqlParser(PqlLexer lexer)
        {
            this.lexer = lexer;
            currentToken = lexer.GetNextToken();
            isTestFile = false;
            statementCounter = 0;
        }

        private void Reset()
        {
            lexer.Reset();
            currentToken = lexer.GetNextToken();
            isTestFile = false;
        }

        private void Eat(PqlTokenType type)
        {
            if (currentToken.Type == type)
            {
                currentToken = lexer.GetNextToken();
            }
            else
            {
                //throw new PqlParserException("Niespodziewany token. Spodziewany: " + type.ToString() + " Rzeczywisty: " + currentToken.Type.ToString(), lexer.lineCounter, lexer.rowCounter);
            }
        }

        public PqlAst Parse()
        {
            PqlAst root = Root() as PqlAst;
            //if (currentToken.Type != PqlTokenType.EOF)
            //throw new PqlParserException("Plik nie został przetworzony do końca", lexer.lineCounter, lexer.rowCounter);
            return root;
        }

        private PqlAst Root()
        {
            throw new NotImplementedException();
        }

        private PqlAst Tuple()
        {
            if (currentToken.Type == PqlTokenType.LTRIAN)
            {
                List<PqlAst> elems = new List<PqlAst>();
                Eat(PqlTokenType.LTRIAN);
                elems.Add(Elem());
                while (currentToken.Type == PqlTokenType.COMMA)
                {
                    Eat(PqlTokenType.COMMA);
                    elems.Add(Elem());
                }
                Eat(PqlTokenType.RTRIAN);

                return new PqlTuple(elems.ToArray());
            }
            else
            {
                return Elem();
            }
        }

        private PqlAst Elem()
        {
            PqlToken id = currentToken;
            Eat(PqlTokenType.IDENT);
            if (currentToken.Type == PqlTokenType.DOT)
            {
                PqlToken attrRef = currentToken;
                Eat(PqlTokenType.ATTRIBUTE);
                return new PqlAttrRef(id, attrRef);
            }
            return new PqlSynonym(id);
        }

        private PqlAst SelectCl()
        {
            List<PqlAst> declarations = Declarations();

            Eat(PqlTokenType.SELECT);
            PqlAst result;
            if (currentToken.Type == PqlTokenType.BOOLEAN)
            {
                result = new PqlBoolean();
                Eat(PqlTokenType.BOOLEAN);
            }
            else
            {
                result = Tuple();
            }

            List<PqlAst> clauses = new List<PqlAst>();

            while (currentToken.Type == PqlTokenType.SUCH
                || currentToken.Type == PqlTokenType.WITH
                || currentToken.Type == PqlTokenType.PATTERN)
            {
                switch (currentToken.Type)
                {
                    case PqlTokenType.SUCH:
                        clauses.Add(SuchThat());
                        break;
                    case PqlTokenType.WITH:
                        clauses.Add(With());
                        break;
                    case PqlTokenType.PATTERN:
                        clauses.Add(Pattern());
                        break;
                    default:
                        throw new Exception();
                }
            }

            return new PqlSelect(declarations, result, clauses);
        }

        private List<PqlAst> Declarations()
        {
            List<PqlAst> declarations = new List<PqlAst>();
            while (currentToken.Type != PqlTokenType.)
            {
                declarations.Add(Declaration());
            }

            return declarations;
        }
    }
}
