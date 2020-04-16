using ParserawkaCore.PQL.AstElements;
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
            List<PqlDeclaration> declarations = Declarations();

            Eat(PqlTokenType.SELECT);
            PqlResult result = ResultCl();

            List<PqlClause> clauses = new List<PqlClause>();

            while (currentToken.Type == PqlTokenType.SUCH
                || currentToken.Type == PqlTokenType.WITH
                || currentToken.Type == PqlTokenType.PATTERN)
            {
                switch (currentToken.Type)
                {
                    case PqlTokenType.SUCH:
                        clauses.Add(SuchThatCl());
                        break;
                    case PqlTokenType.WITH:
                        clauses.Add(WithCl());
                        break;
                    case PqlTokenType.PATTERN:
                        clauses.Add(PatternCl());
                        break;
                    default:
                        throw new Exception();
                }
            }

            return new PqlSelect(declarations, result, clauses);
        }

        private List<PqlDeclaration> Declarations()
        {
            List<PqlDeclaration> declarations = new List<PqlDeclaration>();
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
                            declarations.Add(Declaration(declarationType));
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
            Eat(PqlTokenType.DOT);
            PqlToken attrRef = currentToken;
            Eat(PqlTokenType.ATTRIBUTE);
            return new PqlAttrRef(id, attrRef);
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
                    return Modifies();
				case PqlTokenType.USES:
                    return Uses();
				case PqlTokenType.CALLS:
                    return Calls();
				case PqlTokenType.CALLST:
                    return CallsT();
				case PqlTokenType.PARENT:
                    return Parent();
				case PqlTokenType.PARENTT:
                    return ParentT();
				case PqlTokenType.FOLLOWS:
                    return Follows();
				case PqlTokenType.FOLLOWST:
                    return FollowsT();
				case PqlTokenType.NEXT:
                    return Next();
				case PqlTokenType.NEXTT:
                    return NextT();
				case PqlTokenType.AFFECTS:
                    return Affects();
				case PqlTokenType.AFFECTST:
                    return AffectsT();
                default:
                    throw new Exception();
            }
		}
		
		private PqlModifies Modifies()
		{
			Eat(PqlTokenType.MODIFIES);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlModifies(leftRef, rightRef);
		}
		
		private PqlUses Uses()
		{
			Eat(PqlTokenType.USES);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlUses(leftRef, rightRef);
		}
		
		private PqlCalls Calls()
		{
			Eat(PqlTokenType.CALLS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlCalls(leftRef, rightRef);
		}
		
		private PqlCallsT CallsT()
		{
			Eat(PqlTokenType.CALLST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlCallsT(leftRef, rightRef);
		}
		
		private PqlParent Parent()
		{
			Eat(PqlTokenType.PARENT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlParent(leftRef, rightRef);
		}
		
		private PqlParentT ParentT()
		{
			Eat(PqlTokenType.PARENTT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlParentT(leftRef, rightRef);
		}
		
		private PqlFollows Follows()
		{
			Eat(PqlTokenType.FOLLOWS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlFollows(leftRef, rightRef);
		}
		
		private PqlFollowsT FollowsT()
		{
			Eat(PqlTokenType.FOLLOWST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);

			return new PqlFollowsT(leftRef, rightRef);
		}
		
		private PqlNext Next()
		{
			Eat(PqlTokenType.NEXT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlNext(leftRef, rightRef);
		}
		
		private PqlNextT NextT()
		{
			Eat(PqlTokenType.NEXTT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlNextT(leftRef, rightRef);
		}
		
		private PqlAffects Affects()
		{
			Eat(PqlTokenType.AFFECTS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);

			
			return new PqlAffects(leftRef, rightRef);
		}
		
		private PqlAffectsT AffectsT()
		{
			Eat(PqlTokenType.AFFECTST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlAffectsT(leftRef, rightRef);
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
