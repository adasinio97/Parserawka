using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.Model
{
    public class NextTable : INextTable
    {
        public IStatementList GetNext(Statement statement)
        {
            if (statement != null)
                return statement.NextedBy.Copy();
            else
                return ImplementationFactory.CreateStatementList();
        }

        public IStatementList GetNextedBy(Statement statement)
        {
            if (statement != null)
                return statement.Nexting.Copy();
            else
                return ImplementationFactory.CreateStatementList();
        }

        public IStatementList GetNextedByT(Statement statement)
        {
            IStatementList nexted = GetNextedBy(statement);
            for (int i = 0; i < nexted.GetSize(); i++)
                nexted.Sum(GetNextedBy(nexted[i]));
            return nexted;
        }

        public IStatementList GetNextT(Statement statement)
        {
            IStatementList nexting = GetNext(statement);
            for (int i = 0; i < nexting.GetSize(); i++)
                nexting.Sum(GetNext(nexting[i]));
            return nexting;
        }

        public List<IStatementList> GetPathsFrom(Statement statement)
        {
            List<IStatementList> paths = new List<IStatementList>();
            IStatementList newPath = ImplementationFactory.CreateStatementList();
            BuildPath(paths, newPath, statement);
            return paths;
        }

        private void BuildPath(List<IStatementList> paths, IStatementList path, Statement lastElement)
        {
            Statement element = path.GetSize() > 0 ? path.GetLast() : lastElement;
            if (path.GetSize() == 0 || element != lastElement)
            {
                IStatementList nexting = GetNext(element);
                if (nexting.GetSize() > 0)
                {
                    if (nexting.GetLast() != nexting.GetFirst())
                    {
                        IStatementList newPath = path.Copy();
                        newPath.AddStatement(nexting.GetLast());
                        BuildPath(paths, newPath, element);
                    }
                    path.AddStatement(nexting.GetFirst());
                    BuildPath(paths, path, element);
                }
                else
                    paths.Add(path);
            }
            else
                paths.Add(path);
        }

        public bool IsNext(Statement statement1, Statement statement2)
        {
            IStatementList statementList = GetNext(statement1);
            return statementList.Contains(statement2);
        }

        public bool IsNextT(Statement statement1, Statement statement2)
        {
            IStatementList statementList = GetNextT(statement1);
            return statementList.Contains(statement2);
        }

        public void SetNext(Statement statement1, Statement statement2)
        {
            statement1.NextedBy.AddStatement(statement2);
            statement2.Nexting.AddStatement(statement1);
        }
    }
}
