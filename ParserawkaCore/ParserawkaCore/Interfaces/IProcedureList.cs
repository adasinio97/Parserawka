using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Interfaces
{
    public interface IProcedureList : IEnumerable<Procedure>
    {
        Procedure this[int i] { get; }

        int AddProcedure(Procedure procedure);
        Procedure GetProcedureByIndex(int index);
        Procedure GetProcedureByName(string name);
        int GetIndex(Procedure procedure);
        int GetIndexByName(string name);
        int GetSize();
        bool Contains(Procedure procedure);
        bool Contains(string name);
        IProcedureList Intersection(IProcedureList otherProcedureList);
    }
}
