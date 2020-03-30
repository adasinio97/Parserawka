using ParserawkaWPF.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaWPF.Model
{
    class UsesTable : IUsesTable
    {
        List<Uses> UsesList = new List<Uses>();
        public IVariableList GetUsedBy(Statement statement)
        {
            return UsesList.Where(x => x.Statement == statement).FirstOrDefault().Variable.VariableList;
        }

        public IStatementList GetUses(Variable variable)
        {
            return UsesList.Where(x => x.Variable == variable).FirstOrDefault().Statement.StatementList;
        }

        public bool IsUses(Statement statement, Variable variable)
        {
            IStatementList statementList = GetUses(variable);
            for(int i = 0; i < statementList.GetSize(); i++)
            {
                if(statementList.GetStatementByIndex(i) == statement)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetUses(Statement statement, Variable variable)
        {
            UsesList.Add(new Uses(statement, variable));
        }
    }
}
