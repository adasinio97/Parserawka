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
        /* Lista do wyszukiwania po indeksach, słownik do wyszukiwania po nazwach. */
        private List<Variable> list;
        private Dictionary<string, Variable> dictionary;

        public VariableList()
        {
            dictionary = new Dictionary<string, Variable>();
            list = new List<Variable>();
        }

        public int AddVariable(Variable variable)
        {
            int index;
            Variable existingVariable = GetVariableByName(variable.Name);
            if (existingVariable != null)
                index = GetIndex(existingVariable);
            else
            {
                index = list.Count;
                dictionary.Add(variable.Name, variable);
                list.Add(variable);
            }
            return index;
        }

        public int GetIndex(Variable variable)
        {
            return list.IndexOf(variable);
        }

        public int GetIndexByName(string name)
        {
            return GetIndex(GetVariableByName(name));
        }

        public int GetSize()
        {
            return list.Count;
        }

        public Variable GetVariableByIndex(int index)
        {
            return list[index];
        }

        public Variable GetVariableByName(string name)
        {
            Variable output = null;
            dictionary.TryGetValue(name, out output);
            return output;
        }
    }
}
