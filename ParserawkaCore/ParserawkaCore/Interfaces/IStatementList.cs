using System.Collections.Generic;
using ParserawkaCore.Model;

namespace ParserawkaCore.Interfaces
{
    public interface IStatementList : IEnumerable<Statement>
    {
        Statement this[int i] { get; }

        int AddStatement(Statement statement);
        Statement GetStatementByIndex(int index);
        Statement GetStatementByProgramLine(int programLine);
        int GetIndex(Statement statement);
        int GetIndexByProgramLine(int programLine);
        int GetSize();
        bool Contains(Statement statement);
        bool Contains(int programLine);
        IStatementList Intersection(IStatementList otherStatementList);
    }
}
