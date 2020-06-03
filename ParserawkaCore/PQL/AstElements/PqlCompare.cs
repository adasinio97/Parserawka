using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.PQL.AstElements
{
    public class PqlCompare : PqlAst
    {
        public PqlAttrRef LeftRef { get; set; }
        public PqlArgument RightRef { get; set; }

        public IEntityList LeftArgs { get; private set; }
        public IEntityList RightArgs { get; private set; }

        private PqlTokenType LeftType { get; set; }
        private PqlTokenType RightType { get; set; }

        public PqlCompare(PqlAttrRef leftRef, PqlArgument rightRef)
        {
            LeftRef = leftRef;
            RightRef = rightRef;
        }

        public void Process(IProgramKnowledgeBase pkb, BindingsManager bindingsManager)
        {
            IEntityList leftBounds = ImplementationFactory.CreateEntityList();
            for (int i = 0; i < RightArgs.GetSize(); i++)
            {
                IEntity arg = RightArgs[i];
                IEntityList result = ImplementationFactory.CreateEntityList();
                string attributeValue;
                if (RightType == PqlTokenType.CALL && (LeftType == PqlTokenType.PROCEDURE || LeftType == PqlTokenType.VARIABLE))
                    attributeValue = arg.SecondaryAttribute.AttributeValue;
                else
                    attributeValue = arg.Attribute.AttributeValue;

                if (LeftType == PqlTokenType.CALL && LeftRef.AttributeName.Equals("procName"))
                    result.Sum(pkb.Statements.Copy().FilterBySecondaryAttribute(attributeValue));
                else if (LeftType == PqlTokenType.PROCEDURE)
                    result.AddEntity(pkb.Procedures.GetProcedureByName(attributeValue));
                else if (LeftType == PqlTokenType.VARIABLE)
                    result.AddEntity(pkb.Variables.GetVariableByName(attributeValue));
                else
                    result.AddEntity(pkb.Statements.GetEntityByAttribute(attributeValue));

                if (RightRef is PqlSynonym)
                    bindingsManager.CreateMultipleBindingsOneWay(arg, result, RightArgs, LeftArgs);
                leftBounds.Sum(result);
            }
            LeftArgs.Intersection(leftBounds, bindingsManager);
        }

        public void LoadArgs(IProgramKnowledgeBase pkb, IDeclarationList declarations)
        {
            PqlDeclaration declaration = declarations.GetDeclarationBySynonym(LeftRef.SynonymName);
            LeftArgs = declaration.EntityList;
            LeftArgs.ListName = LeftRef.SynonymName;
            LeftType = declaration.DesignEntity.Type;

            if (RightRef is PqlInteger || RightRef is PqlString)
                RightArgs = LoadSingleRightArg(pkb);
            else
            {
                string synonym = RightRef is PqlAttrRef ? (RightRef as PqlAttrRef).SynonymName : (RightRef as PqlSynonym).Name;
                declaration = declarations.GetDeclarationBySynonym(synonym);
                RightArgs = declaration.EntityList;
                RightArgs.ListName = synonym;
                RightType = declaration.DesignEntity.Type;
            }
        }

        private IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb)
        {
            IEntity entity;
            if (RightRef is PqlInteger)
                entity = pkb.Statements.GetEntityByAttribute((RightRef as PqlInteger).Value);
            else
            {
                entity = pkb.Procedures.GetEntityByAttribute((RightRef as PqlString).Value);
                if (entity == null)
                    entity = pkb.Variables.GetEntityByAttribute((RightRef as PqlString).Value);
            }
            IEntityList result = ImplementationFactory.CreateEntityList();
            result.AddEntity(entity);
            return result;
        }
    }
}
