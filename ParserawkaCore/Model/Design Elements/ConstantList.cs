using ParserawkaCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class ConstantList : EntityList, IConstantList
    {
        public new Constant this[int i] => base[i] as Constant;

        public int AddConstant(Constant constant)
        {
            return AddEntity(constant);
        }

        public bool Contains(Constant constant)
        {
            return base.Contains(constant);
        }

        public bool Contains(int value)
        {
            return Contains(value.ToString());
        }

        public Constant GetConstantByIndex(int index)
        {
            return GetEntityByIndex(index) as Constant;
        }

        public Constant GetConstantByValue(int value)
        {
            return GetEntityByAttribute(value.ToString()) as Constant;
        }

        public int GetIndex(Constant constant)
        {
            return base.GetIndex(constant);
        }

        public int GetIndexByValue(int value)
        {
            return GetIndexByAttribute(value.ToString());
        }

        public new IEnumerator<Constant> GetEnumerator()
        {
            foreach (IEntity entity in list)
                yield return entity as Constant;
        }

        IEnumerator<Constant> IEnumerable<Constant>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
