using ParserawkaWPF.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace ParserawkaWPF.Model
{
    public class StatementList : IStatementList
    {
        private List<Statement> list;
        private Dictionary<int, Statement> dictionary;

        public StatementList()
        {
            list = new List<Statement>();
            dictionary = new Dictionary<int, Statement>();
        }

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

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
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
            Statement output = null;
            dictionary.TryGetValue(programLine, out output);
            return output;
        }

        IEnumerator<Statement> IEnumerable<Statement>.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
