using ParserawkaWPF.Parser.AstElements;
using ParserawkaWPF.Parser.Exceptions;
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
            List<AST> declarations = Procedures();
            AstStmtLst root = new AstStmtLst();
            foreach (AST node in declarations)
            {
                root.children.Add(node);
            }
            return root;
        }

        public List<AST> Procedures()
        {
            AST procedure = Procedure();
            List<AST> retList = new List<AST>();
            retList.Add(procedure);

            while (currentToken.Type == TokenType.PROCEDURE)
            {
                retList.Add(Procedure());
            }
            return retList;
        }

        public AST Procedure()
        {
            Eat(TokenType.PROCEDURE);
            string name = currentToken.Value.ToString();
            Eat(TokenType.ID); //nazwa testu

            AstProcedure test = new AstProcedure(name, StmtLst());
            isTestFile = true;
            return test;
        }

        public AST StmtLst()
        {
            Eat(TokenType.LBRACE);
            List<AST> statements = Statements();
            Eat(TokenType.RBRACE);

            AstStmtLst root = new AstStmtLst();
            root.children.AddRange(statements);

            return root;
        }

        public List<AST> Statements()
        {
            AST statement = SingleStatement();
            List<AST> retList = new List<AST>();
            retList.Add(statement);

            while (currentToken.Type != TokenType.RBRACE )
            {
                retList.Add(SingleStatement());
            }
            return retList;
        }

        public AST SingleStatement()
        {
            if (currentToken.Type == TokenType.ID)
                return Assignment();
            else if (currentToken.Type == TokenType.IF)
                return IfStatement();
            else if (currentToken.Type == TokenType.WHILE)
                return WhileStatement();
            else if (currentToken.Type == TokenType.CALL)
                return CallStatement();
            else if (currentToken.Type == TokenType.ID)
                throw new ParserException("Nie rozpoznano instrukcji \"" + currentToken.Value.ToString() + "\"", lexer.lineCounter, lexer.rowCounter);
            else
                return new AstEmpty();
        }

        public Token GetAssignType()
        {
            Token operation = currentToken;
            if (currentToken.Type == TokenType.ASSIGN)
                Eat(TokenType.ASSIGN);
            return operation;
        }

        public AST IfStatement()
        {
            Eat(TokenType.IF);
            AST var = Var();
            Eat(TokenType.THEN);
            AST body = StmtLst();
            AST elseBody = new AstEmpty();
            if (currentToken.Type == TokenType.ELSE)
            {
                elseBody = StmtLst();
            }

            return new AstIfStatement(var, body, elseBody);
        }

        public AST WhileStatement()
        {
            Eat(TokenType.WHILE);
            AST var = Var();
            Eat(TokenType.THEN);
            AST body = StmtLst();

            return new AstWhileStatement(var, body);
        }

        public AST Assignment()
        {
            AstVariable left = Var();
            Token token = currentToken;
            Eat(TokenType.ASSIGN);
            AST right = Expression();
            Eat(TokenType.SEMI);

            return new AstAssign(left, token, right);
        }

        public AST CallStatement()
        {
            Token token = currentToken;
            Eat(TokenType.ID);
            Eat(TokenType.SEMI);

            return new AstCall(token);
        }

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
            else if (token.Type == TokenType.INTEGER)
            {
                Eat(TokenType.INTEGER);

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
            while (currentToken.Type == TokenType.MUL)
            {
                Token Operator = currentToken;
                Eat(TokenType.MUL);

                node = new AstBinOp(node, Operator, Factor());
            }

            return node;
        }

        public AST Expression()
        {
            AST node = Term();
            while (currentToken.Type == TokenType.PLUS
                || currentToken.Type == TokenType.MINUS)
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
                node = new AstBinOp(node, Operator, Term());
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
