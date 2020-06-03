using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.PQL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Utils
{
    /* Używajcie zamiast operatora new, kiedy potrzebna jest instancja typu interfejsowego.
     * Łatwiej będzie ogarniać kod, kiedy nie ma jeszcze implementacji, albo jeśli okaże się, że wypadałoby przetestować kilka możliwych wariantów. */
    public class ImplementationFactory
    {
        public static IProgramKnowledgeBase CreateProgramKnowledgeBase()
        {
            return ProgramKnowledgeBase.GetInstance();
        }

        public static IDesignExtractor CreateDesignExtractor()
        {
            return new DesignExtractor();
        }

        public static IStatementList CreateStatementList()
        {
            return new StatementList();
        }

        public static IVariableList CreateVariableList()
        {
            return new VariableList();
        }

        public static IProcedureList CreateProcedureList()
        {
            return new ProcedureList();
        }

        public static IConstantList CreateConstantList()
        {
            return new ConstantList();
        }

        public static IFollowsTable CreateFollowsTable()
        {
            return new FollowsTable();
        }

        public static IModifiesTable CreateModifiesTable()
        {
            return new ModifiesTable();
        }

        public static IParentTable CreateParentTable()
        {
            return new ParentTable();
        }

        public static IUsesTable CreateUsesTable()
        {
            return new UsesTable();
        }

        public static ICallsTable CreateCallsTable()
        {
            return new CallsTable();
        }

        public static INextTable CreateNextTable()
        {
            return new NextTable();
        }

        public static IAffectsTable CreateAffectsTable()
        {
            return new AffectsTable();
        }

        public static IDeclarationList CreateDeclarationList()
        {
            return new DeclarationList();
        }

        public static IEntityList CreateEntityList()
        {
            return new EntityList();
        }
    }
}
