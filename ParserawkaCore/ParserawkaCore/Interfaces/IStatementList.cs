using System.Collections.Generic;
using ParserawkaCore.Model;

namespace ParserawkaCore.Interfaces
{
    public interface IStatementList : IEntityList, IEnumerable<Statement>
    {
        new Statement this[int i] { get; }

        int AddStatement(Statement statement);
        Statement GetStatementByIndex(int index);
        Statement GetStatementByProgramLine(int programLine);
        Statement GetFirst();
        Statement GetLast();
        int GetIndex(Statement statement);
        int GetIndexByProgramLine(int programLine);
        bool Contains(Statement statement);
        bool Contains(int programLine);
 
        new IEnumerator<Statement> GetEnumerator();
    }
}
