using System;
using ParserawkaCore.Model;
using System.Collections.Generic;

namespace ParserawkaCore.Interfaces
{
    public interface IVariableList : IEnumerable<Variable>
    {
        Variable this[int i] { get; }

        int AddVariable(Variable variable);
        Variable GetVariableByIndex(int index);
        Variable GetVariableByName(string name);
        int GetIndex(Variable variable);
        int GetIndexByName(string name);
        int GetSize();
        bool Contains(Variable variable);
        bool Contains(string name);
        IVariableList Intersection(IVariableList otherVariableList);
    }
}
