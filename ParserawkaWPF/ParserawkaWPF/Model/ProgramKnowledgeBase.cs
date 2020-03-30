using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ParserawkaWPF.Interfaces;

namespace ParserawkaWPF.Model
{
    class ProgramKnowledgeBase : IProgramKnowledgeBase
    {
        public IVariableList Variables { get; set; }

        public IStatementList Statements { get; set; }

        public IFollowsTable FollowsTable { get; set; }

        public IParentTable ParentTable { get; set; }

        public IModifiesTable ModifiesTable { get; set; }

        public IUsesTable UsesTable { get; set; }

        public void LoadData(string ProgramName)
        {
            StreamReader sr = new StreamReader(ProgramName);
            char [] buffor = new char[sr.BaseStream.Length];
            sr.ReadBlock(buffor, 0, (int)sr.BaseStream.Length);
        }
    }
}
