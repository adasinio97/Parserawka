using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface IConstantList : IEntityList, IEnumerable<Constant>
    {
        new Constant this[int i] { get; }

        int AddConstant(Constant constant);
        Constant GetConstantByIndex(int index);
        Constant GetConstantByValue(int value);
        int GetIndex(Constant constant);
        int GetIndexByValue(int value);
        bool Contains(Constant constant);
        bool Contains(int value);

        new IEnumerator<Constant> GetEnumerator();
    }
}
