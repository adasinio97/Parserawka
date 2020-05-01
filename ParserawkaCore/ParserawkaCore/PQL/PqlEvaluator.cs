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
                ProcessWith(with);
            ProcessTypes();
            foreach (PqlSuchThat suchThat in select.SuchThatClauses)
                ProcessSuchThat(suchThat);
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
            foreach (PqlRelation relation in suchThat.RelCond)
            {
                relation.LoadArgs(PKB, Declarations);
                relation.Process(PKB); // Większość logiki z pętli przeniesiona do PqlRelation.Process()

                if (relation.LeftArgs.GetSize() == 0 || relation.RightArgs.GetSize() == 0)
                {
                    resultBoolean = false;
                    return;
                }
            }
            resultBoolean = true;
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
