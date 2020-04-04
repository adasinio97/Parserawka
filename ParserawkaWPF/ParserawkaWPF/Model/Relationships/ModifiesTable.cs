using ParserawkaWPF.Interfaces;
using ParserawkaWPF.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ParserawkaWPF.Model
{
    class ModifiesTable : IModifiesTable
    {
        private List<Modify> modifyList = new List<Modify>();

        public IVariableList GetModifiedBy(Statement statement)
        {
            List<Modify> list = modifyList.Where(x => x.Statement == statement).ToList();
            IVariableList variableList = ImplementationFactory.CreateVariableList();

            for (int i = 0; i < list.Count; i++)
            {
                variableList.AddVariable(list[i].Variable);
            }
            return variableList;
        }
        
        public IStatementList GetModifies(Variable variable)
        {
            List<Modify> lista = modifyList.Where(x => x.Variable == variable).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList(); 

            for(int i = 0; i < lista.Count; i++)
            {
                statementList.AddStatement(lista[i].Statement);
            }
            return statementList;
        }
        
        public bool IsModified(Statement statement, Variable variable)
        {
            IStatementList statementList = GetModifies(variable);
            return statementList.Contains(statement);
        }
        
        public void SetModifies(Statement statement, Variable variable)
        {
            modifyList.Add(new Modify(statement, variable));
        }
    }
}
