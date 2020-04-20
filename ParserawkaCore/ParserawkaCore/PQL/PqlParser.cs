using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL
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
            PqlAst root = Root();
            //if (currentToken.Type != PqlTokenType.EOF)
            //throw new PqlParserException("Plik nie został przetworzony do końca", lexer.lineCounter, lexer.rowCounter);
            return root;
        }

        private PqlAst Root()
        {
            return SelectCl();
        }

        private PqlTuple Tuple()
        {
            List<PqlElem> elems = new List<PqlElem>();
            if (currentToken.Type == PqlTokenType.LTRIAN)
            {
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
                elems.Add(Elem());
                return new PqlTuple(elems.ToArray());
            }
        }

        private PqlElem Elem()
        {
            PqlToken id = currentToken;
            Eat(PqlTokenType.IDENT);
            if (currentToken.Type == PqlTokenType.DOT)
            {
                Eat(PqlTokenType.DOT);
                PqlToken attrRef = currentToken;
                Eat(PqlTokenType.ATTRIBUTE);
                return new PqlAttrRef(id, attrRef);
            }
            return new PqlSynonym(id);
        }

        private PqlSelect SelectCl()
        {
            IDeclarationList declarations = Declarations();

            Eat(PqlTokenType.SELECT);
            PqlResult result = ResultCl();

            List<PqlWith> withClauses = new List<PqlWith>();
            List<PqlSuchThat> suchThatClauses = new List<PqlSuchThat>();
            List<PqlPattern> patternClauses = new List<PqlPattern>();

            while (currentToken.Type == PqlTokenType.SUCH
                || currentToken.Type == PqlTokenType.WITH
                || currentToken.Type == PqlTokenType.PATTERN)
            {
                switch (currentToken.Type)
                {
                    case PqlTokenType.SUCH:
                        suchThatClauses.Add(SuchThatCl());
                        break;
                    case PqlTokenType.WITH:
                        withClauses.Add(WithCl());
                        break;
                    case PqlTokenType.PATTERN:
                        patternClauses.Add(PatternCl());
                        break;
                    default:
                        throw new Exception();
                }
            }

            return new PqlSelect(declarations, result, withClauses, suchThatClauses, patternClauses);
        }

        private IDeclarationList Declarations()
        {
            IDeclarationList declarations = ImplementationFactory.CreateDeclarationList();
            PqlToken declarationType;

            while (currentToken.Type != PqlTokenType.SELECT)
            {
                switch (currentToken.Type)
                {
                    case PqlTokenType.PROCEDURE:
                    case PqlTokenType.STMTLST:
                    case PqlTokenType.STMT:
                    case PqlTokenType.ASSIGN:
                    case PqlTokenType.CALL:
                    case PqlTokenType.WHILE:
                    case PqlTokenType.IF:
                    case PqlTokenType.VARIABLE:
                    case PqlTokenType.CONSTANT:
                    case PqlTokenType.PROG_LINE:
                        declarationType = currentToken;
                        Eat(currentToken.Type);
                        while (currentToken.Type != PqlTokenType.SEMI)
                            declarations.AddDeclaration(Declaration(declarationType));
                        Eat(PqlTokenType.SEMI);
                        break;
                    default:
                        throw new Exception();
                }
            }

            return declarations;
        }

        private PqlDeclaration Declaration(PqlToken declarationType)
        {
            PqlToken id = currentToken;
            PqlSynonym synonym = new PqlSynonym(id);
            Eat(PqlTokenType.IDENT);
            if (currentToken.Type == PqlTokenType.COMMA)
                Eat(PqlTokenType.COMMA);
            return new PqlDeclaration(declarationType, synonym);
        }
		
		private PqlResult ResultCl()
		{
			if(currentToken.Type == PqlTokenType.BOOLEAN)
			{
				Eat(PqlTokenType.BOOLEAN);
				return new PqlBoolean();
			}
			return Tuple();
		}
		
		private PqlWith WithCl()
		{
			Eat(PqlTokenType.WITH);
			return new PqlWith(AttrCond());
		}
		
		private PqlSuchThat SuchThatCl()
		{
			Eat(PqlTokenType.SUCH);
			Eat(PqlTokenType.THAT);
			return new PqlSuchThat(RelCond());
		}
		
		private PqlPattern PatternCl()
		{
			Eat(PqlTokenType.PATTERN);
			return new PqlPattern(PatternCond());
		}
		
		private List<PqlCompare> AttrCond()
		{
			List<PqlCompare> attrCompare = new List<PqlCompare>();
			attrCompare.Add(AttrCompare());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				attrCompare.Add(AttrCompare());
			}
			
			return attrCompare;
		}
		
		private PqlCompare AttrCompare()
		{
			PqlAttrRef left = AttrRef();
			Eat(PqlTokenType.EQ);
			PqlArgument right = Ref();
			return new PqlCompare(left, right);
		}
		
		private PqlArgument Ref()
		{
			PqlToken id = currentToken;
            if (currentToken.Type == PqlTokenType.IDENT)
            {
                Eat(PqlTokenType.IDENT);
                return new PqlSynonym(id);
            }
            else if (currentToken.Type == PqlTokenType.INTEGER)
            {
                Eat(PqlTokenType.INTEGER);
                return new PqlInteger(id);
            }
            else if (currentToken.Type == PqlTokenType.QUOT)
            {
                Eat(PqlTokenType.QUOT);
                id = currentToken;
                Eat(currentToken.Type);
                Eat(PqlTokenType.QUOT);
                return new PqlString(id);
            }
            else if (currentToken.Type == PqlTokenType.FLOOR)
            {
                Eat(PqlTokenType.FLOOR);
                return new PqlEmptyArg();
            }
            throw new Exception();
		}
		
		private PqlAttrRef AttrRef()
		{
            PqlToken id = currentToken;
            Eat(PqlTokenType.IDENT);
            if (currentToken.Type == PqlTokenType.DOT)
            {
                Eat(PqlTokenType.DOT);
                PqlToken attrRef = currentToken;
                Eat(PqlTokenType.ATTRIBUTE);
                return new PqlAttrRef(id, attrRef);
            }
            return new PqlAttrRef(id);
		}
		
		private List<PqlRelation> RelCond()
		{
			List<PqlRelation> relRef = new List<PqlRelation>();
			relRef.Add(RelRef());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				relRef.Add(RelRef());
			}
			
			return relRef;
		}
		
		private PqlRelation RelRef()
		{
			switch (currentToken.Type)
            {
				case PqlTokenType.MODIFIES:
				case PqlTokenType.USES:
				case PqlTokenType.CALLS:
				case PqlTokenType.CALLST:
				case PqlTokenType.PARENT:
				case PqlTokenType.PARENTT:
				case PqlTokenType.FOLLOWS:
				case PqlTokenType.FOLLOWST:
				case PqlTokenType.NEXT:
				case PqlTokenType.NEXTT:
				case PqlTokenType.AFFECTS:
				case PqlTokenType.AFFECTST:
                    return Relation(currentToken.Type);
                default:
                    throw new Exception();
            }
		}

        private PqlRelation Relation(PqlTokenType relationType)
        {
            Eat(relationType);
            Eat(PqlTokenType.LPAREN);
            PqlArgument leftRef = Ref();
            Eat(PqlTokenType.COMMA);
            PqlArgument rightRef = Ref();
            Eat(PqlTokenType.RPAREN);
            return new PqlRelation(relationType, leftRef, rightRef);
        }
		
		private List<PqlPatternCond> PatternCond()
		{
			List<PqlPatternCond> pattern = new List<PqlPatternCond>();
			pattern.Add(Pattern());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				pattern.Add(Pattern());
			}
			
			return pattern;
		}
		
		private PqlPatternCond Pattern()
		{
            return null;
            // Zostawmy to na później
            /*
			switch(currentToken.Type)
			{
				case PqlTokenType.ASSIGN:
					return Assign();
				case PqlTokenType.WHILE:
					return While();
				case PqlTokenType.IF:
					return If();
				default:
                    throw new Exception();
			} */
		}	
    }
}
