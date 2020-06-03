using ParserawkaCore.Interfaces;
using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    public class ProcedureList : EntityList, IProcedureList, AST
    {
        public ProcedureList() : base() { }

        public new Procedure this[int i] => base[i] as Procedure;

        public int AddProcedure(Procedure procedure)
        {
            return AddEntity(procedure);
        }

        public override IEntityList CreateNewList()
        {
            return ImplementationFactory.CreateProcedureList();
        }

        public int GetIndexByName(string name)
        {
            return GetIndexByAttribute(name);
        }

        public Procedure GetProcedureByIndex(int index)
        {
            return GetEntityByIndex(index) as Procedure;
        }

        public Procedure GetProcedureByName(string name)
        {
            return GetEntityByAttribute(name) as Procedure;
        }

        public new bool Contains(string name)
        {
            return base.Contains(name);
        }

        public bool Contains(Procedure procedure)
        {
            return base.Contains(procedure);
        }

        public int GetIndex(Procedure procedure)
        {
            return base.GetIndex(procedure);
        }

        public new IEnumerator<Procedure> GetEnumerator()
        {
            foreach (IEntity entity in list)
                yield return entity as Procedure;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public new IProcedureList Copy()
        {
            return base.Copy() as IProcedureList;
        }
    }
}
