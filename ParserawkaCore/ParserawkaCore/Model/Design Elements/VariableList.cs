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
    public class VariableList : EntityList, IVariableList
    {
        public VariableList() : base() { }

        public new Variable this[int i] => base[i] as Variable;

        public int AddVariable(Variable variable)
        {
            return AddEntity(variable);
        }

        public override IEntityList CreateNewList()
        {
            return ImplementationFactory.CreateVariableList();
        }

        public int GetIndexByName(string name)
        {
            return GetIndexByAttribute(name);
        }

        public Variable GetVariableByIndex(int index)
        {
            return GetEntityByIndex(index) as Variable;
        }

        public Variable GetVariableByName(string name)
        {
            return GetEntityByAttribute(name) as Variable;
        }

        public new bool Contains(string name)
        {
            return base.Contains(name);
        }

        public bool Contains(Variable variable)
        {
            return base.Contains(variable);
        }

        public new IEnumerator<Variable> GetEnumerator()
        {
            foreach (IEntity entity in list)
                yield return entity as Variable;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetIndex(Variable variable)
        {
            return base.GetIndex(variable);
        }
    }
}
