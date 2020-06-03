using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Interfaces
{
    public interface IProcedureList : IEntityList, IEnumerable<Procedure>
    {
        new Procedure this[int i] { get; }

        int AddProcedure(Procedure procedure);
        Procedure GetProcedureByIndex(int index);
        Procedure GetProcedureByName(string name);
        int GetIndex(Procedure procedure);
        int GetIndexByName(string name);
        bool Contains(Procedure procedure);
        new bool Contains(string name);

        new IEnumerator<Procedure> GetEnumerator();
        new IProcedureList Copy();
    }
}
