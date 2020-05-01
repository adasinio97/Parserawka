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
    public abstract class PqlRelation : PqlAst
    {
        public PqlArgument LeftRef { get; private set; }
        public PqlArgument RightRef { get; private set; }

        public PqlTokenType RelationType { get; private set; }

        public IEntityList LeftArgs { get; private set; }
        public IEntityList RightArgs { get; private set; }

        public PqlRelation(PqlTokenType relationType, PqlArgument leftRef, PqlArgument rightRef)
        {
            RelationType = relationType;
            LeftRef = leftRef;
            RightRef = rightRef;
        }

        public void LoadArgs(IProgramKnowledgeBase pkb, IDeclarationList declarations)
        {
            if (LeftRef is PqlInteger || LeftRef is PqlString)
                LeftArgs = LoadSingleLeftArg(pkb);
            else if (LeftRef is PqlEmptyArg)
                LeftArgs = LoadLeftArgs(pkb);
            else
            {
                PqlDeclaration declaration = declarations.GetDeclarationBySynonym((LeftRef as PqlSynonym).Name);
                LeftArgs = declaration.EntityList;
            }

            if (RightRef is PqlInteger || RightRef is PqlString)
                RightArgs = LoadSingleRightArg(pkb);
            else if (RightRef is PqlEmptyArg)
                RightArgs = LoadRightArgs(pkb);
            else
            {
                PqlDeclaration declaration = declarations.GetDeclarationBySynonym((RightRef as PqlSynonym).Name);
                RightArgs = declaration.EntityList;
            }
        }

        public void Process(IProgramKnowledgeBase pkb)
        {
            if (LeftArgs.GetSize() < RightArgs.GetSize())
            {
                IEntityList rightBounds = ImplementationFactory.CreateEntityList();
                for (int i = 0; i < LeftArgs.GetSize(); i++)
                    rightBounds.Sum(ProcessLeftSide(pkb, LeftArgs[i]));
                RightArgs.Intersection(rightBounds);

                IEntityList leftBounds = ImplementationFactory.CreateEntityList();
                for (int i = 0; i < RightArgs.GetSize(); i++)
                    leftBounds.Sum(ProcessRightSide(pkb, RightArgs[i]));
                LeftArgs.Intersection(leftBounds);
            }
            else
            {
                IEntityList leftBounds = ImplementationFactory.CreateEntityList();
                for (int i = 0; i < RightArgs.GetSize(); i++)
                    leftBounds.Sum(ProcessRightSide(pkb, RightArgs[i]));
                LeftArgs.Intersection(leftBounds);

                IEntityList rightBounds = ImplementationFactory.CreateEntityList();
                for (int i = 0; i < LeftArgs.GetSize(); i++)
                    rightBounds.Sum(ProcessLeftSide(pkb, LeftArgs[i]));
                RightArgs.Intersection(rightBounds);
            }
        }

        protected abstract IEntityList LoadSingleLeftArg(IProgramKnowledgeBase pkb);

        protected abstract IEntityList LoadLeftArgs(IProgramKnowledgeBase pkb);

        protected abstract IEntityList LoadSingleRightArg(IProgramKnowledgeBase pkb);

        protected abstract IEntityList LoadRightArgs(IProgramKnowledgeBase pkb);

        protected abstract IEntityList ProcessLeftSide(IProgramKnowledgeBase pkb, IEntity arg);

        protected abstract IEntityList ProcessRightSide(IProgramKnowledgeBase pkb, IEntity arg);

        public abstract bool CheckFull(IProgramKnowledgeBase pkb, IEntity arg1, IEntity arg2);
    }
}
