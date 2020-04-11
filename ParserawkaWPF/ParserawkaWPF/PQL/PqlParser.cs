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
                return AttrRef();
            }
            return new PqlSynonym(id);
        }

        private PqlAst SelectCl()
        {
            List<PqlAst> declarations = Declarations();

            Eat(PqlTokenType.SELECT);
            PqlAst result = ResultCl();

            List<PqlAst> clauses = new List<PqlAst>();

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

        private List<PqlAst> Declarations()
        {
            List<PqlAst> declarations = new List<PqlAst>();
            while (currentToken.Type != PqlTokenType.)
            {
                declarations.Add(Declaration());
            }

            return declarations;
        }
		
		private PqlAst ResultCl()
		{
			if(currentToken.Type == PqlTokenType.BOOLEAN)
			{
				Eat(PqlTokenType.BOOLEAN);
				return new PqlBoolean();
			}
			return Tuple();
		}
		
		private PqlAst WithCl()
		{
			Eat(PqlTokenType.WITH);
			return AttrCond();
		}
		
		private PqlAst SuchThatCl()
		{
			Eat(PqlTokenType.SUCH);
			Eat(PqlTokenType.THAT);
			return RelCond();
		}
		
		private PqlAst PatternCl()
		{
			Eat(PqlTokenType.PATTERN);
			return PatternCond();
		}
		
		private List<PqlAst> AttrCond()
		{
			List<PqlAst> attrCompare = new List<PqlAst>();
			attrCompare.Add(AttrCompare());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				attrCompare.Add(AttrCompare());
			}
			
			return attrCompare;
		}
		
		private PqlAst AttrCompare()
		{
			PqlAst left = Ref();
			Eat(PqlTokenType.EQ);
			PqlAst right = Ref();
			return new PqlCompare(left, right);
		}
		
		private PqlAst Ref()
		{
			PqlToken id = currentToken;
			if(currentToken.Type == PqlTokenType.IDENT)
			{
				return new PqlSynonym(id);
			} else if(currentToken.Type == PqlTokenType.INTEGER)
			{
				return new PqlInteger(id);
			} else if(currentToken.Type == PqlTokenType.QUOT)
			{
				Eat(PqlTokenType.QUOT);
				id = currentToken();
				Eat(PqlTokenType.QUOT);
				return id;
			} 
			return AttRef();
		}
		
		private PqlAst AttrRef()
		{
			PqlToken attrRef = currentToken;
            Eat(PqlTokenType.ATTRIBUTE);
            return new PqlAttrRef(id, attrRef);
		}
		
		private List<PqlAst> RelCond()
		{
			List<PqlAst> relRef = new List<PqlAst>();
			relRef.Add(RelRef());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				relRef.Add(AttrCompare());
			}
			
			return relRef;
		}
		
		private PqlAst RelRef()
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
		
		private PqlAst Modifies()
		{
			Eat(PqlTokenType.MODIFIES);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlModifies(leftRef, rightRef);
		}
		
		private PqlAst Uses()
		{
			Eat(PqlTokenType.Uses);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlUses(leftRef, rightRef);
		}
		
		private PqlAst Calls()
		{
			Eat(PqlTokenType.CALLS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlCalls(leftRef, rightRef);
		}
		
		private PqlAst CallsT()
		{
			Eat(PqlTokenType.CALLST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlCallsT(leftRef, rightRef);
		}
		
		private PqlAst Parent()
		{
			Eat(PqlTokenType.PARENT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlParent(leftRef, rightRef);
		}
		
		private PqlAst ParentT()
		{
			Eat(PqlTokenType.PARENTT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlParentT(leftRef, rightRef);
		}
		
		private PqlAst Follows()
		{
			Eat(PqlTokenType.FOLLOWS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlFollows(leftRef, rightRef);
		}
		
		private PqlAst FollowsT()
		{
			Eat(PqlTokenType.FOLLOWST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);

			return new PqlFollowsT(leftRef, rightRef);
		}
		
		private PqlAst Next()
		{
			Eat(PqlTokenType.NEXT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlNext(leftRef, rightRef);
		}
		
		private PqlAst NextT()
		{
			Eat(PqlTokenType.NEXTT);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlNextT(leftRef, rightRef);
		}
		
		private PqlAst Affects()
		{
			Eat(PqlTokenType.AFFECTS);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);

			
			return new PqlAffects(leftRef, rightRef);
		}
		
		private PqlAst AffectsT()
		{
			Eat(PqlTokenType.AFFECTST);
			Eat(PqlTokenType.LPAREN);
			PqlAst leftRef = Ref();
			Eat(PqlTokenType.COMMA);
			PqlAst rightRef = Ref();
			Eat(PqlTokenType.RPAREN);
			
			return new PqlAffectsT(leftRef, rightRef);
		}
		
		private PqlAst patternCond()
		{
			List<PqlAst> pattern = new List<PqlAst>();
			pattern.Add(Pattern());
			while (currentToken.Type == PqlTokenType.AND)
			{
				Eat(PqlTokenType.AND);
				attrCompare.Add(Pattern());
			}
			
			return pattern;
		}
		
		private PqlAst Pattern()
		{
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
			}
		}	
    }
}
