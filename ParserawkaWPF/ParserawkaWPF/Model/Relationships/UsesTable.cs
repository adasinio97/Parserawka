using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaWPF.Model
{
    class UsesTable : IUsesTable
    {
        List<Uses> UsesList = new List<Uses>();
        public IVariableList GetUsedBy(Statement statement)
        {
            List<Uses> list = UsesList.Where(x => x.Statement == statement).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();

            for (int i = 0; i < list.Count; i++)
            {
                variableList.AddVariable(list[i].Variable);
            }
            return variableList;
        }

        public IStatementList GetUses(Variable variable)
        {
            List<Uses> list =  UsesList.Where(x => x.Variable == variable).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            for (int i = 0; i< list.Count; i++)
            {
                statementList.AddStatement(list[i].Statement);
            }
            return statementList;
        }

        public bool IsUses(Statement statement, Variable variable)
        {
            IStatementList statementList = GetUses(variable);
            return statementList.Contains(statement);
        }

        public void SetUses(Statement statement, Variable variable)
        {
            UsesList.Add(new Uses(statement, variable));
        }
    }
}
