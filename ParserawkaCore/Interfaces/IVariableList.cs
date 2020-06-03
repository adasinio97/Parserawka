using System;
using ParserawkaCore.Model;
using System.Collections.Generic;

namespace ParserawkaCore.Interfaces
{
    public interface IVariableList : IEntityList, IEnumerable<Variable>
    {
        new Variable this[int i] { get; }

        int AddVariable(Variable variable);
        Variable GetVariableByIndex(int index);
        Variable GetVariableByName(string name);
        int GetIndex(Variable variable);
        int GetIndexByName(string name);
        bool Contains(Variable variable);
        new bool Contains(string name);

        new IEnumerator<Variable> GetEnumerator();
        new IVariableList Copy();
    }
}
