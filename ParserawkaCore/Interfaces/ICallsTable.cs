using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Interfaces
{
    public interface ICallsTable
    {
        void SetCalls(Procedure procedure1, Procedure procedure2);
        IProcedureList GetCalling(Procedure procedure);
        IProcedureList GetCallingT(Procedure procedure);
        IProcedureList GetCalledBy(Procedure procedure);
        IProcedureList GetCalledByT(Procedure procedure);
        bool IsCalls(Procedure procedure1, Procedure procedure2);
        bool IsCallsT(Procedure procedure1, Procedure procedure2);
    }
}
