using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Model;
using ParserawkaWPF.Parser.AstElements;
using ParserawkaWPF.Parser.Exceptions;
using ParserawkaWPF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser
{
    public class Parser : IParser
    {
        public Lexer lexer { get; set; }

        private int statementCounter;
        private bool isTestFile;

        private Token currentToken;

        public Parser(Lexer lexer)
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

        private void Eat(TokenType type)
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

        private IProcedureList Root()
        {
            IProcedureList root = Procedures();
            return root;
        }

        private IProcedureList Procedures()
        {
            Procedure procedure = Procedure();
            IProcedureList procedures = ImplementationFactory.CreateProcedureList();
            procedures.AddProcedure(procedure);

            while (currentToken.Type == TokenType.PROCEDURE)
            {
                procedures.AddProcedure(Procedure());
            }
            return procedures;
        }

        private Procedure Procedure()
        {
            Eat(TokenType.PROCEDURE);
            string name = currentToken.Value.ToString();
            Eat(TokenType.ID); //nazwa testu

            Procedure test = new Procedure(name, StmtLst());
            isTestFile = true;
            return test;
        }

        private IStatementList StmtLst()
        {
            Eat(TokenType.LBRACE);
            IStatementList statements = Statements();
            Eat(TokenType.RBRACE);

            IStatementList root = ImplementationFactory.CreateStatementList();
            foreach (Statement statement in statements)
                root.AddStatement(statement);

            return root;
        }

        private IStatementList Statements()
        {
            Statement statement = SingleStatement();
            IStatementList statements = ImplementationFactory.CreateStatementList();
            statements.AddStatement(statement);

            while (currentToken.Type != TokenType.RBRACE )
            {
                statements.AddStatement(SingleStatement());
            }
            return statements;
        }

        private Statement SingleStatement()
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
                return null;
        }

        private Token GetAssignType()
        {
            Token operation = currentToken;
            if (currentToken.Type == TokenType.ASSIGN)
                Eat(TokenType.ASSIGN);
            return operation;
        }

        private If IfStatement()
        {
            int programLine = ++statementCounter;

            Eat(TokenType.IF);
            Variable var = Var();
            Eat(TokenType.THEN);

            IStatementList body = StmtLst();
            IStatementList elseBody = null;
            if (currentToken.Type == TokenType.ELSE)
            {
                elseBody = StmtLst();
            }
            return new If(var, body, elseBody, programLine);
        }

        private While WhileStatement()
        {
            int programLine = ++statementCounter;

            Eat(TokenType.WHILE);
            Variable var = Var();
            Eat(TokenType.THEN);

            IStatementList body = StmtLst();

            return new While(var, body, programLine);
        }

        private Assign Assignment()
        {
            Variable left = Var();
            Token token = currentToken;
            Eat(TokenType.ASSIGN);
            Factor right = Expression();
            Eat(TokenType.SEMI);

            return new Assign(left, token, right, ++statementCounter);
        }

        private Call CallStatement()
        {
            Token token = currentToken;
            Eat(TokenType.ID);
            Eat(TokenType.SEMI);

            return new Call(token, ++statementCounter);
        }

        private Variable Var()
        {
            Variable variable = new Variable(currentToken);
            Eat(TokenType.ID);
            return variable;
        }

        private Factor Factor()
        {
            Token token = currentToken;
            if (token.Type == TokenType.ID)
            {
                return Var();
            }
            else if (token.Type == TokenType.INTEGER)
            {
                Eat(TokenType.INTEGER);

                return new Constant(token);
            }
            else if (token.Type == TokenType.LPAREN)
            {
                Eat(TokenType.LPAREN);
                Factor expr = Expression();
                Eat(TokenType.RPAREN);

                return expr;
            }
            throw new ParserException("Nieprawidłowe określenie \"" + token.Value.ToString() + "\"", lexer.lineCounter, lexer.rowCounter);
        }

        private Factor Term()
        {
            Factor node = Factor();
            while (currentToken.Type == TokenType.MUL)
            {
                Token Operator = currentToken;
                Eat(TokenType.MUL);

                node = new Expression(node, Operator, Factor());
            }

            return node;
        }

        private Factor Expression()
        {
            Factor node = Term();
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
                node = new Expression(node, Operator, Term());
            }

            return node;
        }

        public AST Parse()
        {
            AST root = Root() as AST;
            if (currentToken.Type != TokenType.EOF)
                throw new ParserException("Plik nie został przetworzony do końca", lexer.lineCounter, lexer.rowCounter);
            return root;
        }
    }
}
