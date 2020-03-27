using ParserawkaWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class StatementList : IStatementList
    {
        private Dictionary<int, Statement> dictionary;

        public StatementList()
        {
            dictionary = new Dictionary<int, Statement>();
        }

        public int AddStatement(Statement statement)
        {
            int index = dictionary.Count;
            dictionary.Add(statement.ProgramLine, statement);
            return index;
        }

        public int GetIndex(Statement statement)
        {
            return dictionary.Keys.ToList().IndexOf(statement.ProgramLine);
        }

        public int GetIndexByProgramLine(int programLine)
        {
            return GetIndex(GetStatementByProgramLine(programLine));
        }

        public int GetSize()
        {
            return dictionary.Count;
        }

        public Statement GetStatementByIndex(int index)
        {
            return dictionary.Values.ToList()[index];
        }

        public Statement GetStatementByProgramLine(int programLine)
        {
            Statement output = null;
            dictionary.TryGetValue(programLine, out output);
            return output;
        }
    }
}
