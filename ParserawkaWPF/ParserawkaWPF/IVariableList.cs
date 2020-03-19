using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF
{
    interface IVariableList
    {
        int addVariable(Variable variable);
        Variable getVariableByIndex(int index);
        Variable getVariableByName(String name);
        int getIndex(Variable variable);
        int getIndexByName(String name);
        int getSize();
    }
}
