using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.PQL.Model;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL
{
    public class PqlEvaluator
    {
        private IDeclarationList Declarations { get; set; }
        private IProgramKnowledgeBase PKB { get; set; }
        private PqlAst QueryTree { get; set; }
        private bool resultBoolean;

        public PqlOutput Output { get; private set; }

        public PqlEvaluator(IProgramKnowledgeBase pkb, PqlAst queryTree)
        {
            PKB = pkb;
            QueryTree = queryTree;
        }

        public PqlOutput Evaluate()
        {
            ProcessSelect();
            return Output;
        }

        private void ProcessSelect()
        {
            PqlSelect select = QueryTree as PqlSelect;
            ProcessDeclarations(select.Declarations);

            foreach (PqlWith with in select.WithClauses)
            {
                ProcessWith(with);
            }

            ProcessTypes();

            foreach (PqlSuchThat suchThat in select.SuchThatClauses)
            {
                ProcessSuchThat(suchThat);
            }

            foreach(PqlPatternCond patternCond in select.PatternClauses)
            {
                ProcessPattern(patternCond);
            }
            Output = ProcessResult(select.Result);
        }

        private void ProcessPattern(PqlPatternCond patternCond)
        {
            foreach(PqlPatternNode pn in patternCond.PatternNode)
            {
                if (Declarations.GetDeclarationBySynonym(pn.Synonym.Value.ToString()).DeclarationType.IsAssignableFrom(typeof(Assign)))
                {
                    ProcessAssignPattern(pn);
                }
                else if (Declarations.GetDeclarationBySynonym(pn.Synonym.Value.ToString()).DeclarationType.IsAssignableFrom(typeof(While)))
                {
                    ProcessWhilePattern(pn);
                }
                else if (Declarations.GetDeclarationBySynonym(pn.Synonym.Value.ToString()).DeclarationType.IsAssignableFrom(typeof(If)))
                {
                    ProcessIfPattern(pn);
                }
            }
        }

        private void ProcessIfPattern(PqlPatternNode pn)
        {
            throw new NotImplementedException();
        }

        private void ProcessWhilePattern(PqlPatternNode pn)
        {
            throw new NotImplementedException();
        }

        private void ProcessAssignPattern(PqlPatternNode pn)
        {
            throw new NotImplementedException();
        }

        private void ProcessDeclarations(IDeclarationList declarations)
        {
            foreach (PqlDeclaration declaration in declarations)
            {
                switch (declaration.DesignEntity.Type)
                {
                    case PqlTokenType.PROCEDURE:
                        declaration.EntityList = PKB.Procedures.Copy() as EntityList;
                        break;
                    case PqlTokenType.VARIABLE:
                        declaration.EntityList = PKB.Variables.Copy() as EntityList;
                        break;
                    case PqlTokenType.CONSTANT:
                        declaration.EntityList = PKB.Constants.Copy() as EntityList;
                        break;
                    default:
                        declaration.EntityList = PKB.Statements.Copy() as EntityList;
                        break;
                }
            }
            Declarations = declarations;
        }

        private void ProcessWith(PqlWith with)
        {
            foreach (PqlCompare compare in with.AttrCond)
            {
                PqlAttrRef left = compare.LeftRef;
                PqlArgument right = compare.RightRef;
                string attributeValue = null;

                if (right is PqlString)
                    attributeValue = (right as PqlString).Value;
                else if (right is PqlInteger)
                    attributeValue = (right as PqlInteger).Value;

                PqlDeclaration declaration = Declarations.GetDeclarationBySynonym(left.SynonymName);
                declaration.EntityList.FilterByAttribute(attributeValue);
            }
        }

        private void ProcessTypes()
        {
            foreach (PqlDeclaration declaration in Declarations)
                declaration.EntityList.FilterByType(declaration.DeclarationType);
        }

        private void ProcessSuchThat(PqlSuchThat suchThat)
        {
            PqlRelationProcessor rel = new PqlRelationProcessor(PKB);
            foreach (PqlRelation relation in suchThat.RelCond)
            {
                PqlArgument leftArgDef = relation.LeftRef;
                PqlArgument rightArgDef = relation.RightRef;
                IEntityList leftArgs, rightArgs;

                if (leftArgDef is PqlInteger || leftArgDef is PqlString)
                {
                    IEntity leftArg = rel.SelectEntityFromPKBLeft(relation.RelationType, leftArgDef);
                    leftArgs = ImplementationFactory.CreateEntityList();
                    leftArgs.AddEntity(leftArg);
                }
                else if (leftArgDef is PqlEmptyArg)
                    leftArgs = rel.SelectListFromPKBLeft(relation.RelationType);
                else
                {
                    PqlDeclaration declaration = Declarations.GetDeclarationBySynonym((leftArgDef as PqlSynonym).Name);
                    leftArgs = declaration.EntityList;
                }

                if (rightArgDef is PqlInteger || rightArgDef is PqlString)
                {
                    IEntity rightArg = rel.SelectEntityFromPKBRight(relation.RelationType, rightArgDef);
                    rightArgs = ImplementationFactory.CreateEntityList();
                    rightArgs.AddEntity(rightArg);
                }
                else if (rightArgDef is PqlEmptyArg)
                    rightArgs = rel.SelectListFromPKBRight(relation.RelationType);
                else
                {
                    PqlDeclaration declaration = Declarations.GetDeclarationBySynonym((rightArgDef as PqlSynonym).Name);
                    rightArgs = declaration.EntityList;
                }

                if (leftArgs.GetSize() < rightArgs.GetSize())
                {
                    IEntityList rightBounds = ImplementationFactory.CreateEntityList();
                    for (int i = 0; i < leftArgs.GetSize(); i++)
                        rightBounds.Sum(rel.RelationLeft(relation.RelationType, leftArgs[i]));
                    rightArgs.Intersection(rightBounds);

                    IEntityList leftBounds = ImplementationFactory.CreateEntityList();
                    for (int i = 0; i < rightArgs.GetSize(); i++)
                        leftBounds.Sum(rel.RelationRight(relation.RelationType, rightArgs[i]));
                    leftArgs.Intersection(leftBounds);
                }
                else
                {
                    IEntityList leftBounds = ImplementationFactory.CreateEntityList();
                    for (int i = 0; i < rightArgs.GetSize(); i++)
                        leftBounds.Sum(rel.RelationRight(relation.RelationType, rightArgs[i]));
                    leftArgs.Intersection(leftBounds);

                    IEntityList rightBounds = ImplementationFactory.CreateEntityList();
                    for (int i = 0; i < leftArgs.GetSize(); i++)
                        rightBounds.Sum(rel.RelationLeft(relation.RelationType, leftArgs[i]));
                    rightArgs.Intersection(rightBounds);
                }

                if (leftArgs.GetSize() == 0 || rightArgs.GetSize() == 0)
                {
                    resultBoolean = false;
                    return;
                }
            }
            resultBoolean = true;
        }

        private PqlOutput ProcessResult(PqlResult result)
        {
            if (result is PqlBoolean)
                return new PqlBooleanOutput(resultBoolean);
            else
            {
                PqlTuple tuple = result as PqlTuple;
                PqlTupleOutput output = new PqlTupleOutput();
                foreach (PqlElem elem in tuple.Elems)
                {
                    string synonym;
                    if (elem is PqlAttrRef)
                        synonym = (elem as PqlAttrRef).SynonymName;
                    else
                        synonym = (elem as PqlSynonym).Name;
                    PqlDeclaration declaration = Declarations.GetDeclarationBySynonym(synonym);
                    output.Declarations.AddDeclaration(declaration);
                }
                return output;
            }
        }
    }
}
