using System;
using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IVariableList
    {
        int AddVariable(Variable variable);
        Variable GetVariableByIndex(int index);
        Variable GetVariableByName(String name);
        int GetIndex(Variable variable);
        int GetIndexByName(String name);
        int GetSize();
    }
}
