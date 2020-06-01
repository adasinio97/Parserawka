using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaCore.Model
{
    public class CallsTable : ICallsTable
    {
        public IProcedureList GetCalledBy(Procedure procedure)
        {
            if (procedure != null)
                return procedure.Calling.Copy();
            else
                return ImplementationFactory.CreateProcedureList();
        }

        public IProcedureList GetCalledByT(Procedure procedure)
        {
            IProcedureList called = GetCalledBy(procedure);
            for (int i = 0; i < called.GetSize(); i++)
                called.Sum(GetCalledBy(called[i]));
            return called;
        }

        public IProcedureList GetCalling(Procedure procedure)
        {
            if (procedure != null)
                return procedure.CalledBy.Copy();
            else
                return ImplementationFactory.CreateProcedureList();
        }

        public IProcedureList GetCallingT(Procedure procedure)
        {
            IProcedureList calling = GetCalling(procedure);
            for (int i = 0; i < calling.GetSize(); i++)
                calling.Sum(GetCalling(calling[i]));
            return calling;
        }

        public bool IsCalls(Procedure procedure1, Procedure procedure2)
        {
            IProcedureList procedureList = GetCalling(procedure2);
            return procedureList.Contains(procedure1);
        }

        public bool IsCallsT(Procedure procedure1, Procedure procedure2)
        {
            IProcedureList callingProcedures = GetCallingT(procedure2);
            return callingProcedures.Contains(procedure1);
        }

        public void SetCalls(Procedure procedure1, Procedure procedure2)
        {
            procedure1.Calling.AddProcedure(procedure2);
            procedure2.CalledBy.AddProcedure(procedure1);
        }
    }
}
