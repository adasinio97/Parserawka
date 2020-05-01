using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface INextTable
    {
        void SetNext(Statement statement1, Statement statement2);
        IStatementList GetNext(Statement statement);
        IStatementList GetNextT(Statement statement);
        IStatementList GetNextedBy(Statement statement);
        IStatementList GetNextedByT(Statement statement);
        bool IsNext(Statement statement1, Statement statement2);
        bool IsNextT(Statement statement1, Statement statement2);
    }
}
