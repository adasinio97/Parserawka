using System.Collections.Generic;
using System.Linq;
using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;

namespace ParserawkaCore.Model
{
    class ParentTable : IParentTable
    {
        public Statement GetParent(Statement statement)
        {
            return statement?.Parent;
        }

        public IStatementList GetParentedBy(Statement statement)
        {
            if (statement != null)
                return statement.Children.Copy();
            else
                return ImplementationFactory.CreateStatementList();
        }

        public IStatementList GetParentedByT(Statement statement)
        {
            IStatementList parented = GetParentedBy(statement);
            for (int i = 0; i < parented.GetSize(); i++)
                parented.Sum(GetParentedBy(parented[i]));
            return parented;
        }

        public IStatementList GetParentT(Statement statement)
        {
            IStatementList parents = ImplementationFactory.CreateStatementList();
            Statement parentStatement = GetParent(statement);
            parents.AddStatement(parentStatement);
            for (int i = 0; i < parents.GetSize(); i++)
                parents.AddEntity(GetParent(parents[i]));
            return parents;
        }

        public bool IsParent(Statement firstStatement, Statement secondStatement)
        {
            return GetParent(secondStatement) == firstStatement;
        }

        public bool IsParentT(Statement firstStatement, Statement secondStatement)
        {
            IStatementList parents = GetParentT(secondStatement);
            return parents.Contains(firstStatement);
        }

        public void SetParent(Statement firstStatement, Statement secondStatement)
        {
            firstStatement.Children.AddStatement(secondStatement);
            secondStatement.Parent = firstStatement;
        }
    }
}
