using ParserawkaCore.Interfaces;
using ParserawkaCore.Model;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.Model
{
    public class DeclarationList : EntityList, IDeclarationList
    {
        public DeclarationList() : base() { }

        public new PqlDeclaration this[int i] => base[i] as PqlDeclaration;

        public int AddDeclaration(PqlDeclaration declaration)
        {
            return AddEntity(declaration);
        }

        public override IEntityList CreateNewList()
        {
            return ImplementationFactory.CreateDeclarationList();
        }

        public PqlDeclaration GetDeclarationByIndex(int index)
        {
            return GetEntityByIndex(index) as PqlDeclaration;
        }

        public PqlDeclaration GetDeclarationBySynonym(string synonym)
        {
            return GetEntityByAttribute(synonym) as PqlDeclaration;
        }

        public int GetIndexBySynonym(string synonym)
        {
            return GetIndexByAttribute(synonym);
        }

        public new bool Contains(string synonym)
        {
            return base.Contains(synonym);
        }

        public bool Contains(PqlDeclaration declaration)
        {
            return base.Contains(declaration);
        }

        public new IEnumerator<PqlDeclaration> GetEnumerator()
        {
            foreach (IEntity entity in list)
                yield return entity as PqlDeclaration;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
