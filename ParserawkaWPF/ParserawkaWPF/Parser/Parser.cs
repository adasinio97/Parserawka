using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser
{
    public class Parser
    {
        public Lexer lexer { get; set; }

        public bool isTestFile;

        private Token currentToken;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            currentToken = lexer.GetNextToken();
            isTestFile = false;
        }

        public void Reset()
        {
            lexer.Reset();
            currentToken = lexer.GetNextToken();
            isTestFile = false;
        }

        public void Eat(TokenType type)
        {
            if (currentToken.Type == type)
            {
                currentToken = lexer.GetNextToken();
            }
            else
            {
                throw new ParserException("Niespodziewany token. Spodziewany: " + type.ToString() + " Rzeczywisty: " + currentToken.Type.ToString(), lexer.lineCounter, lexer.rowCounter);
            }
        }

        public AST Root()
        {
            List<AST> declarations = Declarations();
            AstCompound root = new AstCompound();
            foreach (AST node in declarations)
            {
                root.children.Add(node);
            }
            return root;
        }

        public AST Test()
        {
            Eat(TokenType.TEST);
            string name = currentToken.Value.ToString();
            Eat(TokenType.ID); //nazwa testu
            if (currentToken.Type == TokenType.EOL)
                Eat(TokenType.EOL);

            AstTest test = new AstTest(name, Compound());
            isTestFile = true;
            return test;
        }

        public AST Compound()
        {
            Eat(TokenType.LBRACE);
            List<AST> statements = Statements();
            Eat(TokenType.RBRACE);

            AstCompound root = new AstCompound();
            foreach (AST node in statements)
            {
                root.children.Add(node);
            }

            return root;
        }

        public List<AST> Declarations()
        {
            AST declaration = SingleDeclaration();
            List<AST> retList = new List<AST>();
            retList.Add(declaration);

            while (currentToken.Type == TokenType.SEMI || currentToken.Type == TokenType.EOL)
            {
                if (currentToken.Type == TokenType.SEMI)
                    Eat(TokenType.SEMI);
                else
                    Eat(TokenType.EOL);

                retList.Add(SingleDeclaration());
            }
            return retList;
        }

        public List<AST> Statements()
        {
            AST statement = SingleStatement();
            List<AST> retList = new List<AST>();
            retList.Add(statement);

            while (currentToken.Type == TokenType.SEMI || currentToken.Type == TokenType.EOL)
            {
                if (currentToken.Type == TokenType.SEMI)
                    Eat(TokenType.SEMI);
                else
                    Eat(TokenType.EOL);

                retList.Add(SingleStatement());
            }
            return retList;
        }

        public AST SingleDeclaration()
        {
            if (currentToken.Type == TokenType.TYPE)
                return Declaration();
            else if (currentToken.Type == TokenType.IMPORT)
                return Import();
            else if (currentToken.Type == TokenType.TEST)
                return Test();
            else if (currentToken.Type == TokenType.ID)
                throw new ParserException("Nie rozpoznano typu \"" + currentToken.Value.ToString() + "\"", lexer.lineCounter, lexer.rowCounter);
            else
                return new AstEmpty();
        }

        public AST SingleStatement()
        {
            if (currentToken.Type == TokenType.LBRACE)
                return Compound();
            //else if (currentToken.Type == TokenType.TYPE)
            //    return Declaration();
            //else if (currentToken.Type == TokenType.ID)
            //    return Assignment();
            //else if (currentToken.Type == TokenType.IF)
            //    return IfStatement();
            else if (currentToken.Type == TokenType.SET)
                return SetDp();
            else if (currentToken.Type == TokenType.ASSERT)
                return Assert();
            else if (currentToken.Type == TokenType.WAIT)
                return Wait();
            else if (currentToken.Type == TokenType.TIMESET)
                return TimeSet();
            else if (currentToken.Type == TokenType.DATESET)
                return DateSet();
            else if (currentToken.Type == TokenType.ID)
                throw new ParserException("Nie rozpoznano instrukcji \"" + currentToken.Value.ToString() + "\"", lexer.lineCounter, lexer.rowCounter);
            else
                return new AstEmpty();
        }

        public AST DateSet()
        {
            Eat(TokenType.DATESET);
            Token day, month, year;
            //godzina
            day = currentToken;
            Eat(TokenType.INTEGER);
            Eat(TokenType.COMMA);
            //minuta
            month = currentToken;
            Eat(TokenType.INTEGER);
            Eat(TokenType.COMMA);
            //sekunda
            year = currentToken;
            Eat(TokenType.INTEGER);
            return new AstDateSet(day, month, year);
        }

        public AST TimeSet()
        {
            Eat(TokenType.TIMESET);
            Token h, m, s;
            //godzina
            h = currentToken;
            Eat(TokenType.INTEGER);
            Eat(TokenType.COLON);
            //minuta
            m = currentToken;
            Eat(TokenType.INTEGER);
            Eat(TokenType.COLON);
            //sekunda
            s = currentToken;
            Eat(TokenType.INTEGER);
            return new AstTimeSet(h, m, s);
        }

        public Token GetAssignType()
        {
            Token operation = currentToken;
            if (currentToken.Type == TokenType.ASSIGN)
                Eat(TokenType.ASSIGN);
            else if (currentToken.Type == TokenType.PLUSEQ)
                Eat(TokenType.PLUSEQ);
            else if (currentToken.Type == TokenType.MINUSEQ)
                Eat(TokenType.MINUSEQ);
            else if (currentToken.Type == TokenType.MULEQ)
                Eat(TokenType.MULEQ);
            else if (currentToken.Type == TokenType.DIVEQ)
                Eat(TokenType.DIVEQ);
            else if (currentToken.Type == TokenType.BOREQ)
                Eat(TokenType.BOREQ);
            else if (currentToken.Type == TokenType.BANDEQ)
                Eat(TokenType.BANDEQ);
            else if (currentToken.Type == TokenType.BXOREQ)
                Eat(TokenType.BXOREQ);
            else
                Eat(TokenType.ASSIGN); //funkcja wywoła wyjątek
            return operation;
        }

        public AST SetDp()
        {
            Eat(TokenType.SET);
            AstVariable var = Var();
            Token operation = GetAssignType();
            AST expr = Expression();

            return new AstSetParam(var, operation, expr);
        }

        public AST Assert()
        {
            Eat(TokenType.ASSERT);
            AST condition = Condition();
            Eat(TokenType.COLON);
            Token message = currentToken;
            Eat(TokenType.STRING);

            return new AstAssert(condition, message);
        }

        public AST Wait()
        {
            Eat(TokenType.WAIT);
            Token time;
            AST condition = null;
            if (currentToken.Type == TokenType.TIME)
            {
                time = currentToken;
                Eat(TokenType.TIME);
            }
            else
            {
                condition = Condition();
                Eat(TokenType.COLON);
                time = currentToken;
                Eat(TokenType.TIME);
            }
            return new AstWait(condition, time);
        }

        public AST Import()
        {
            Eat(TokenType.IMPORT);
            String fileText;
            string filePath = Properties.Settings.Default.TestDirectory + '/' + currentToken.Value.ToString(); ;
            Eat(TokenType.STRING);
            if (File.Exists(filePath))
            {
                fileText = File.ReadAllText(filePath);
            }
            else
            {
                throw new ParserException("Plik " + filePath + " nie istnieje", lexer.lineCounter, lexer.rowCounter);
            }
            Lexer subLexer = new Lexer(fileText);
            Parser subParser = new Parser(subLexer);
            return subParser.Parse();
        }

        //public AST IfStatement()
        //{
        //    Eat(TokenType.IF);
        //    AST expr = Expression();
        //    AST body = Compound();
        //    AST elseBody = new AstEmpty();
        //    if(currentToken.Type == TokenType.ELSE)
        //    {
        //        elseBody = Compound();
        //    }

        //    return new AstIfStatement(expr, body, elseBody);
        //}

        public AST Declaration()
        {
            Token type = currentToken;
            Eat(TokenType.TYPE);
            Token id = currentToken;
            Eat(TokenType.ID);
            if (currentToken.Type == TokenType.SEMI || currentToken.Type == TokenType.EOL)
            {
                return new AstDeclaration(type, id, new AstNum(new Token(TokenType.INTEGER, 0)));
            }
            Eat(TokenType.ASSIGN);
            AST expr = Expression();
            return new AstDeclaration(type, id, expr);
        }

        //public AST Assignment()
        //{
        //    AST left = Var();
        //    Token token = currentToken;
        //    Eat(TokenType.ASSIGN);
        //    AST right = Expression();

        //    return new AstAssign((AstVariable)left, token, right);
        //}

        public AstVariable Var()
        {
            AstVariable variable = new AstVariable(currentToken);
            Eat(TokenType.ID);
            return variable;
        }

        public AST Factor()
        {
            Token token = currentToken;
            if (token.Type == TokenType.ID)
            {
                return Var();
            }
            else if (token.Type == TokenType.PLUS)
            {
                Eat(TokenType.PLUS);

                return new AstUnOp(token, Factor());
            }
            else if (token.Type == TokenType.MINUS)
            {
                Eat(TokenType.MINUS);

                return new AstUnOp(token, Factor());
            }
            else if (token.Type == TokenType.NEG)
            {
                Eat(TokenType.NEG);

                return new AstUnOp(token, Factor());
            }
            else if (token.Type == TokenType.BNOT)
            {
                Eat(TokenType.BNOT);

                return new AstUnOp(token, Factor());
            }
            else if (token.Type == TokenType.INTEGER)
            {
                Eat(TokenType.INTEGER);

                return new AstNum(token);
            }
            else if (token.Type == TokenType.FLOAT)
            {
                Eat(TokenType.FLOAT);

                return new AstNum(token);
            }
            else if (token.Type == TokenType.BIT)
            {
                Eat(TokenType.BIT);

                return new AstNum(token);
            }
            else if (token.Type == TokenType.LPAREN)
            {
                Eat(TokenType.LPAREN);
                AST expr = Expression();
                Eat(TokenType.RPAREN);

                return expr;
            }
            throw new ParserException("Nieprawidłowe określenie \"" + token.Value.ToString() + "\"", lexer.lineCounter, lexer.rowCounter);
        }

        public AST Term()
        {
            AST node = Factor();
            while (currentToken.Type == TokenType.MUL || currentToken.Type == TokenType.DIV)
            {
                Token Operator = currentToken;
                if (Operator.Type == TokenType.MUL)
                {
                    Eat(TokenType.MUL);
                }
                else if (Operator.Type == TokenType.DIV)
                {
                    Eat(TokenType.DIV);
                }
                node = new AstBinOp(node, Operator, Factor());
            }

            return node;
        }

        public AST Expression()
        {
            AST node = Term();
            while (currentToken.Type == TokenType.PLUS
                || currentToken.Type == TokenType.MINUS
                || currentToken.Type == TokenType.BOR
                || currentToken.Type == TokenType.BAND
                || currentToken.Type == TokenType.BXOR)
            {
                Token Operator = currentToken;
                if (Operator.Type == TokenType.PLUS)
                {
                    Eat(TokenType.PLUS);
                }
                else if (Operator.Type == TokenType.MINUS)
                {
                    Eat(TokenType.MINUS);
                }
                else if (Operator.Type == TokenType.BOR)
                {
                    Eat(TokenType.BOR);
                }
                else if (Operator.Type == TokenType.BAND)
                {
                    Eat(TokenType.BAND);
                }
                else if (Operator.Type == TokenType.BXOR)
                {
                    Eat(TokenType.BXOR);
                }
                node = new AstBinOp(node, Operator, Term());
            }

            return node;
        }

        public AST Equation()
        {
            AST node = Expression();
            while (currentToken.Type == TokenType.EQ
                || currentToken.Type == TokenType.NEQ
                || currentToken.Type == TokenType.LT
                || currentToken.Type == TokenType.LTEQ
                || currentToken.Type == TokenType.GT
                || currentToken.Type == TokenType.GTEQ)
            {
                Token Operator = currentToken;
                if (Operator.Type == TokenType.EQ)
                {
                    Eat(TokenType.EQ);
                }
                else if (Operator.Type == TokenType.NEQ)
                {
                    Eat(TokenType.NEQ);
                }
                else if (Operator.Type == TokenType.LT)
                {
                    Eat(TokenType.LT);
                }
                else if (Operator.Type == TokenType.LTEQ)
                {
                    Eat(TokenType.LTEQ);
                }
                else if (Operator.Type == TokenType.GT)
                {
                    Eat(TokenType.GT);
                }
                else if (Operator.Type == TokenType.GTEQ)
                {
                    Eat(TokenType.GTEQ);
                }
                node = new AstEquation(node, Operator, Expression());
            }

            return node;
        }

        public AST Condition()
        {
            AST node = Equation();
            while (currentToken.Type == TokenType.AND
                || currentToken.Type == TokenType.OR)
            {
                Token Operator = currentToken;
                if (Operator.Type == TokenType.AND)
                {
                    Eat(TokenType.AND);
                }
                else if (Operator.Type == TokenType.OR)
                {
                    Eat(TokenType.OR);
                }

                node = new AstCondition(node, Operator, Equation());
            }

            return node;
        }

        public AST Parse()
        {
            AST root = Root();
            if (currentToken.Type != TokenType.EOF)
                throw new ParserException("Plik nie został przetworzony do końca", lexer.lineCounter, lexer.rowCounter);
            return root;
        }
    }
}
