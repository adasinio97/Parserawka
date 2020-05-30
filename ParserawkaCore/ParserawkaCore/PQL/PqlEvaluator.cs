using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.PQL.Model;
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
            resultBoolean = true;
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
            ProcessTypes();
            foreach (PqlWith with in select.WithClauses)
                ProcessWith(with);
            if (resultBoolean)
            {
                foreach (PqlSuchThat suchThat in select.SuchThatClauses)
                    ProcessSuchThat(suchThat);
            }
            Output = ProcessResult(select.Result);
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
                PqlDeclaration declaration = Declarations.GetDeclarationBySynonym(left.SynonymName);

                if (right is PqlString || right is PqlInteger)
                {
                    string attributeValue = null;
                    if (right is PqlString)
                        attributeValue = (right as PqlString).Value;
                    else
                        attributeValue = (right as PqlInteger).Value;

                    if (declaration.DesignEntity.Type == PqlTokenType.CALL &&
                        right is PqlString)
                        declaration.EntityList.FilterBySecondaryAttribute(attributeValue);
                    else
                        declaration.EntityList.FilterByAttribute(attributeValue);
                }
                else if (right is PqlAttrRef || right is PqlSynonym)
                {
                    List<string> attributeValues = new List<string>();
                    PqlDeclaration argumentDeclaration;
                    if (right is PqlAttrRef)
                        argumentDeclaration = Declarations.GetDeclarationBySynonym((right as PqlAttrRef).SynonymName);
                    else
                        argumentDeclaration = Declarations.GetDeclarationBySynonym((right as PqlSynonym).Name);

                    if (right is PqlAttrRef &&
                        argumentDeclaration.DesignEntity.Type == PqlTokenType.CALL &&
                        (right as PqlAttrRef).AttributeName.Equals("procName"))
                    {
                        foreach (IEntity entity in argumentDeclaration.EntityList)
                            attributeValues.Add(entity.SecondaryAttribute.AttributeValue);
                    }
                    else
                    {
                        foreach (IEntity entity in argumentDeclaration.EntityList)
                            attributeValues.Add(entity.Attribute.AttributeValue);
                    }

                    if (declaration.DesignEntity.Type == PqlTokenType.CALL &&
                        left.AttributeName.Equals("procName"))
                        declaration.EntityList.FilterBySecondaryAttributes(attributeValues);
                    else
                        declaration.EntityList.FilterByAttributes(attributeValues);
                }

                if (declaration.EntityList.GetSize() == 0)
                {
                    resultBoolean = false;
                    return;
                }
            }
        }

        private void ProcessTypes()
        {
            foreach (PqlDeclaration declaration in Declarations)
                declaration.EntityList.FilterByType(declaration.DeclarationType);
        }

        private void ProcessSuchThat(PqlSuchThat suchThat)
        {
            BindingsManager bindingsManager = new BindingsManager();
            foreach (PqlRelation relation in suchThat.RelCond)
            {
                relation.LoadArgs(PKB, Declarations);
                relation.Process(PKB, bindingsManager);

                if (relation.LeftArgs.GetSize() == 0 || relation.RightArgs.GetSize() == 0)
                {
                    resultBoolean = false;
                    return;
                }
            }
        }

        private void ProcessPattern(PqlPattern pattern)
        {
            // TODO
            throw new NotImplementedException();
        }

        private PqlOutput ProcessResult(PqlResult result)
        {
            if (result is PqlBoolean)
                return new PqlBooleanOutput(resultBoolean);
            else
            {
                PqlTupleOutput output = new PqlTupleOutput();
                if (resultBoolean)
                {
                    PqlTuple tuple = result as PqlTuple;
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
                }
                return output;
            }
        }
    }
}
