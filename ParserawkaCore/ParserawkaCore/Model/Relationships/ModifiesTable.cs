using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ParserawkaCore.Model
{
    class ModifiesTable : IModifiesTable
    {
        private List<Modify> modifyList = new List<Modify>();

        public IVariableList GetModifiedBy(Procedure procedure)
        {
            throw new NotImplementedException();
        }

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

        public IProcedureList GetModifiesProcedures(Variable variable)
        {
            //throw new NotImplementedException();
            //TODO
            return ImplementationFactory.CreateProcedureList();
        }

        public IStatementList GetModifiesStatements(Variable variable)
        {
            List<Modify> lista = modifyList.Where(x => x.Variable == variable).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList(); 

            for(int i = 0; i < lista.Count; i++)
            {
                statementList.AddStatement(lista[i].Statement);
            }
            return statementList;
        }
        
        public bool IsModifies(Statement statement, Variable variable)
        {
            IStatementList statementList = GetModifiesStatements(variable);
            return statementList.Contains(statement);
        }

        public bool IsModifies(Procedure procedure, Variable variable)
        {
            throw new NotImplementedException();
        }

        public void SetModifies(Procedure procedure, Variable variable)
        {
            //throw new NotImplementedException();
        }

        public void SetModifies(Statement statement, Variable variable)
        {
            if (!IsModifies(statement, variable))
                modifyList.Add(new Modify(statement, variable));
        }
    }
}
