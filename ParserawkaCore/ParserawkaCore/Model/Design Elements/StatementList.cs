using ParserawkaCore.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System;
using ParserawkaCore.Utils;
using ParserawkaCore.Parser.AstElements;

namespace ParserawkaCore.Model
{
    public class StatementList : EntityList, IStatementList, AST
    {
        public StatementList() : base() { }

        public new Statement this[int i] => base[i] as Statement;

        public int AddStatement(Statement statement)
        {
            return AddEntity(statement);
        }

        public override IEntityList CreateNewList()
        {
            return ImplementationFactory.CreateStatementList();
        }

        public int GetIndex(Statement statement)
        {
            return base.GetIndex(statement);
        }

        public int GetIndexByProgramLine(int programLine)
        {
            return GetIndexByAttribute(programLine.ToString());
        }

        public Statement GetStatementByIndex(int index)
        {
            return GetEntityByIndex(index) as Statement;
        }

        public Statement GetStatementByProgramLine(int programLine)
        {
            return GetEntityByAttribute(programLine.ToString()) as Statement;
        }

        public bool Contains(int programLine)
        {
            return Contains(programLine.ToString());
        }

        public bool Contains(Statement statement)
        {
            return base.Contains(statement);
        }

        public new IEnumerator<Statement> GetEnumerator()
        {
            foreach (IEntity entity in list)
                yield return entity as Statement;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Statement GetFirst()
        {
            return list[0] as Statement;
        }

        public Statement GetLast()
        {
            return list[list.Count - 1] as Statement;
        }
    }
}
