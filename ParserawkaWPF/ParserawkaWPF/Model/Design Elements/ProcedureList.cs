using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ParserawkaWPF.Utils;

namespace ParserawkaWPF.Model
{
    public class ProcedureList : AST, IProcedureList
    {
        private List<Procedure> list;
        private SortedDictionary<string, Procedure> dictionary;

        public ProcedureList()
        {
            list = new List<Procedure>();
            dictionary = new SortedDictionary<string, Procedure>();
        }

        public Procedure this[int i] { get { return GetProcedureByIndex(i); } }

        public int AddProcedure(Procedure procedure)
        {
            int index;
            Procedure existingProcedure = GetProcedureByName(procedure.Name);
            if (existingProcedure != null)
                index = GetIndex(existingProcedure);
            else
            {
                index = list.Count;
                dictionary.Add(procedure.Name, procedure);
                list.Add(procedure);
            }
            return index;
        }

        public bool Contains(string name)
        {
            return dictionary.ContainsKey(name);
        }

        public bool Contains(Procedure procedure)
        {
            return dictionary.ContainsKey(procedure.Name);
        }

        public int GetIndex(Procedure procedure)
        {
            return list.IndexOf(procedure);
        }

        public int GetIndexByName(string name)
        {
            return GetIndex(GetProcedureByName(name));
        }

        public Procedure GetProcedureByIndex(int index)
        {
            return list[index];
        }

        public Procedure GetProcedureByName(string name)
        {
            Procedure procedure = null;
            dictionary.TryGetValue(name, out procedure);
            return procedure;
        }

        public int GetSize()
        {
            return list.Count;
        }

        public IProcedureList Intersection(IProcedureList otherProcedureList)
        {
            IProcedureList intersection = ImplementationFactory.CreateProcedureList();
            foreach (Procedure procedure in list)
            {
                if (otherProcedureList.Contains(procedure))
                    intersection.AddProcedure(procedure);
            }
            return intersection;
        }

        public IEnumerator<Procedure> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
