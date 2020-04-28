using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaCore.Model
{
    public class CallsTable : ICallsTable
    {
        private List<Calls> callsList = new List<Calls>();

        public IProcedureList GetCalledBy(Procedure procedure)
        {
            List<Calls> list = callsList.Where(x => x.FirstProcedure == procedure).ToList();
            IProcedureList procedureList = ImplementationFactory.CreateProcedureList();

            foreach (Calls calls in list)
                procedureList.AddProcedure(calls.SecondProcedure);

            return procedureList;
        }

        public IProcedureList GetCalledByT(Procedure procedure)
        {
            return new RecursionContext(this).GetCalledByT(procedure);
        }

        public IProcedureList GetCalling(Procedure procedure)
        {
            List<Calls> list = callsList.Where(x => x.SecondProcedure == procedure).ToList();
            IProcedureList procedureList = ImplementationFactory.CreateProcedureList();

            foreach (Calls calls in list)
                procedureList.AddProcedure(calls.FirstProcedure);

            return procedureList;
        }

        public IProcedureList GetCallingT(Procedure procedure)
        {
            return new RecursionContext(this).GetCallingT(procedure);
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
            if (!IsCalls(procedure1, procedure2))
                callsList.Add(new Calls(procedure1, procedure2));
        }

        private class RecursionContext
        {
            private IProcedureList procedureList;
            private CallsTable outer;

            public RecursionContext(CallsTable outer)
            {
                procedureList = ImplementationFactory.CreateProcedureList();
                this.outer = outer;
            }

            public IProcedureList GetCalledByT(Procedure procedure)
            {
                IProcedureList calledList = outer.GetCalledBy(procedure);
                if (calledList != null)
                {
                    foreach (Procedure calledProcedure in calledList)
                    {
                        GetCalledByT(calledProcedure);
                        procedureList.AddProcedure(calledProcedure);
                    }
                }
                return procedureList;
            }

            public IProcedureList GetCallingT(Procedure procedure)
            {
                IProcedureList callingList = outer.GetCalling(procedure);
                if (callingList != null)
                {
                    foreach (Procedure callingProcedure in callingList)
                    {
                        GetCallingT(callingProcedure);
                        procedureList.AddProcedure(callingProcedure);
                    }
                }
                return procedureList;
            }
        }
    }
}
