using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL.AstElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.Interfaces
{
    public interface IDeclarationList : IEntityList, IEnumerable<PqlDeclaration>
    {
        new PqlDeclaration this[int i] { get; }

        int AddDeclaration(PqlDeclaration declaration);
        PqlDeclaration GetDeclarationByIndex(int index);
        PqlDeclaration GetDeclarationBySynonym(string synonym);
        int GetIndexBySynonym(string synonym);
        bool Contains(PqlDeclaration declaration);
        new bool Contains(string synonym);

        new IEnumerator<PqlDeclaration> GetEnumerator();
    }
}
