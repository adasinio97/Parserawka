using ParserawkaWPF.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaWPF.Model
{
    class ModifiesTable : IModifiesTable
    {
        List<Modify> modifyList = new List<Modify>();
        public IVariableList GetModifiedBy(Statement statement)
        {
            return modifyList.Where(x => x.Statement == statement).FirstOrDefault().Variable.VariableList;
        }
        
        public IStatementList GetModifies(Variable variable)
        {
            return modifyList.Where(x => x.Variable == variable).FirstOrDefault().Statement.StatementList;
        }
        
        public bool IsModified(Statement statement, Variable variable)
        {
            IStatementList statementList = GetModifies(variable);
            for(int i = 0; i < statementList.GetSize(); i++)
            {
                if (statementList.GetStatementByIndex(i) == statement)
                    return true;
            }
            return false;
        }

        public void SetModifies(Statement statement, Variable variable)
        {
            modifyList.Add(new Modify(statement, variable));
        }
    }
}
