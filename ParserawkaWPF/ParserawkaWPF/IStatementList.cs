using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF
{
    interface IStatementList
    {
        int addStatement(Statement statement);
        Statement getStatementByIndex(int index);
        Statement getStatementByProgramLine(int programLine);
        int getIndex(Statement statement);
        int getIndexByProgramLine(int programLine);
        int getSize();
    }
}
