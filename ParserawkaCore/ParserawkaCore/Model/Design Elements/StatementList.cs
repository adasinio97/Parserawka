using ParserawkaCore.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System;
using ParserawkaCore.Utils;
using ParserawkaCore.Parser.AstElements;

namespace ParserawkaCore.Model
{
    public class StatementList : AST, IStatementList
    {
        private List<Statement> list;
        private SortedDictionary<int, Statement> dictionary;

        public StatementList()
        {
            list = new List<Statement>();
            dictionary = new SortedDictionary<int, Statement>();
        }

        public Statement this[int i] { get { return GetStatementByIndex(i); } }

        public int AddStatement(Statement statement)
        {
            int index;
            Statement existingStatement = GetStatementByProgramLine(statement.ProgramLine);
            if (existingStatement != null)
                index = GetIndex(existingStatement);
            else
            {
                index = list.Count;
                list.Add(statement);
                dictionary.Add(statement.ProgramLine, statement);
            }
            return index;
        }

        public int GetIndex(Statement statement)
        {
            return list.IndexOf(statement);
        }

        public int GetIndexByProgramLine(int programLine)
        {
            return GetIndex(GetStatementByProgramLine(programLine));
        }

        public int GetSize()
        {
            return list.Count;
        }

        public Statement GetStatementByIndex(int index)
        {
            return list[index];
        }

        public Statement GetStatementByProgramLine(int programLine)
        {
            Statement statement = null;
            dictionary.TryGetValue(programLine, out statement);
            return statement;
        }

        public bool Contains(Statement statement)
        {
            return dictionary.ContainsKey(statement.ProgramLine);
        }

        public bool Contains(int programLine)
        {
            return dictionary.ContainsKey(programLine);
        }

        public IStatementList Intersection(IStatementList otherStatementList)
        {
            IStatementList intersection = ImplementationFactory.CreateStatementList();
            foreach (Statement statement in list)
            {
                if (otherStatementList.Contains(statement))
                    intersection.AddStatement(statement);
            }
            return intersection;
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator<Statement> IEnumerable<Statement>.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
