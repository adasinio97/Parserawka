using ParserawkaWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class VariableList : IVariableList
    {
        private Dictionary<string, Variable> dictionary;

        public VariableList()
        {
            dictionary = new Dictionary<string, Variable>();
        }

        public int AddVariable(Variable variable)
        {
            int index = dictionary.Count;
            dictionary.Add(variable.Name, variable);
            return index;
        }

        public int GetIndex(Variable variable)
        {
            return dictionary.Keys.ToList().IndexOf(variable.Name);
        }

        public int GetIndexByName(string name)
        {
            return GetIndex(GetVariableByName(name));
        }

        public int GetSize()
        {
            return dictionary.Keys.Count;
        }

        public Variable GetVariableByIndex(int index)
        {
            return dictionary.Values.ToList()[index];
        }

        public Variable GetVariableByName(string name)
        {
            Variable output = null;
            dictionary.TryGetValue(name, out output);
            return output;
        }
    }
}
