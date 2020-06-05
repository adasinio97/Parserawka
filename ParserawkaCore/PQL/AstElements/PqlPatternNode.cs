using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlPatternNode : PqlClause
    {
        public PqlSynonym Synonym { get; set; }
        public PqlArgument VarRef { get; set; }
        public PqlExpr Expr { get; set; } //Jeśli pattern jest typu if albo while to będzie tu null

        public IEntityList Args { get; set; }
        private PqlTokenType Type { get; set; }

        public PqlPatternNode(PqlSynonym syn, PqlArgument varRef, PqlExpr expr)
        {
            Synonym = syn;
            VarRef = varRef;
            Expr = expr;
        }

        public void LoadArgs(IProgramKnowledgeBase pkb, IDeclarationList declarations)
        {
            PqlDeclaration declaration = declarations.GetDeclarationBySynonym(Synonym.Name);
            Args = declaration.EntityList;
            Args.ListName = Synonym.Name;
            Type = declaration.DesignEntity.Type;
        }

        public void Process(IProgramKnowledgeBase pkb, BindingsManager bindingsManager)
        {
            if (Type == PqlTokenType.WHILE || Type == PqlTokenType.IF)
                ProcessContainerPattern(pkb, bindingsManager);
            else
                ProcessAssignPattern(pkb, bindingsManager);
        }

        private void ProcessContainerPattern(IProgramKnowledgeBase pkb, BindingsManager bindingsManager)
        {
            PqlArgument leftRef = VarRef;
            if (leftRef is PqlString)
            {
                Variable variable = pkb.Variables.GetVariableByName((leftRef as PqlString).Value);
                for (int i = 0; i < Args.GetSize(); i++)
                {
                    Container container = Args.GetEntityByIndex(i) as Container;
                    if (!container.Condition.Name.Equals(variable.Name))
                    {
                        bindingsManager.RemoveBoundEntity(container, Args);
                        i--;
                    }
                }
            }
        }

        private void ProcessAssignPattern(IProgramKnowledgeBase pkb, BindingsManager bindingsManager)
        {
            PqlArgument leftRef = VarRef;
            PqlExpr rightRef = Expr;

            if (leftRef is PqlString)
            {
                Variable variable = pkb.Variables.GetVariableByName((leftRef as PqlString).Value);
                for (int i = 0; i < Args.GetSize(); i++)
                {
                    Assign assignment = Args.GetEntityByIndex(i) as Assign;
                    if (assignment == null || !assignment.Left.Name.Equals(variable.Name))
                    {
                        bindingsManager.RemoveBoundEntity(assignment, Args);
                        i--;
                    }
                }
            }

            if (rightRef != null)
            {
                for (int i = 0; i < Args.GetSize(); i++)
                {
                    Assign assignment = Args.GetEntityByIndex(i) as Assign;
                    if (assignment == null)
                    {
                        bindingsManager.RemoveBoundEntity(assignment, Args);
                        i--;
                    }
                    else
                    {
                        bool match = Expr.IsExact ? CompareTrees(assignment.Right, Expr.ExprTree) : FindTree(assignment.Right, Expr.ExprTree);
                        if (!match)
                        {
                            bindingsManager.RemoveBoundEntity(assignment, Args);
                            i--;
                        }
                    }
                }
            }
        }

        private bool CompareTrees(Factor node1, Factor node2)
        {
            if (node1 is Variable && node2 is Variable)
            {
                Variable variable1 = node1 as Variable;
                Variable variable2 = node2 as Variable;
                return variable1.Name.Equals(variable2.Name);
            }
            else if (node1 is Constant && node2 is Constant)
            {
                Constant constant1 = node1 as Constant;
                Constant constant2 = node2 as Constant;
                return constant1.Value == constant2.Value;
            }
            else if (node1 is Expression && node2 is Expression)
            {
                Expression expression1 = node1 as Expression;
                Expression expression2 = node2 as Expression;
                if (expression1.Operation.Value.ToString().Equals(expression2.Operation.Value.ToString()))
                    return CompareTrees(expression1.Left, expression2.Left) && CompareTrees(expression1.Right, expression2.Right);
                else
                    return false;
            }
            else
                return false;
        }

        private bool FindTree(Factor parent, Factor child)
        {
            if (parent != null)
            {
                if (CompareTrees(parent, child))
                    return true;
                else if (parent is Expression && child is Expression)
                {
                    Expression parentExpression = parent as Expression;
                    return FindTree(parentExpression.Left, child) || FindTree(parentExpression.Right, child);
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
