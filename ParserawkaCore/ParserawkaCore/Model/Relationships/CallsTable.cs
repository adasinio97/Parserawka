using ParserawkaCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class CallsTable : ICallsTable
    {
        public IProcedureList GetCalledBy(Procedure procedure)
        {
            throw new NotImplementedException();
        }

        public IProcedureList GetCalledByT(Procedure procedure)
        {
            throw new NotImplementedException();
        }

        public IProcedureList GetCalling(Procedure procedure)
        {
            throw new NotImplementedException();
        }

        public IProcedureList GetCallingT(Procedure procedure)
        {
            throw new NotImplementedException();
        }

        public bool IsCalls(Procedure procedure1, Procedure procedure2)
        {
            throw new NotImplementedException();
        }

        public bool IsCallsT(Procedure procedure1, Procedure procedure2)
        {
            throw new NotImplementedException();
        }

        public void SetCalls(Procedure procedure1, Procedure procedure2)
        {
            //throw new NotImplementedException();
        }
    }
}
