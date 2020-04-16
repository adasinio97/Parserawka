using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ParserawkaCore.Model
{
    public class VariableList : IVariableList
    {
        /* Lista do wyszukiwania po indeksach, słownik do wyszukiwania po nazwach. */
        private List<Variable> list;
        private SortedDictionary<string, Variable> dictionary;

        public VariableList()
        {
            dictionary = new SortedDictionary<string, Variable>();
            list = new List<Variable>();
        }

        public Variable this[int i] { get { return GetVariableByIndex(i); } }

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
            Variable variable;
            dictionary.TryGetValue(name, out variable);
            return variable;
        }

        public bool Contains(Variable variable)
        {
            return dictionary.ContainsKey(variable.Name);
        }

        public bool Contains(string name)
        {
            return dictionary.ContainsKey(name);
        }

        public IVariableList Intersection(IVariableList otherVariableList)
        {
            IVariableList intersection = ImplementationFactory.CreateVariableList();
            foreach (Variable variable in list)
            {
                if (otherVariableList.Contains(variable))
                    intersection.AddVariable(variable);
            }
            return intersection;
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
