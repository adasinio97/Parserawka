using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.Model.Relationships
{
    public class AffectsTable : IAffectsTable
    {
        private List<Affects> affectsList = new List<Affects>();

        public IStatementList GetAffectedBy(Assign assignment)
        {
            List<Affects> list = affectsList.Where(x => x.FirstAssignment == assignment).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            foreach (Affects affects in list)
                statementList.AddStatement(affects.SecondAssignment);

            return statementList;
        }

        public IStatementList GetAffectedByT(Assign assignment)
        {
            return new RecursionContext(this).GetAffectedByT(assignment);
        }

        public IStatementList GetAffects(Assign assignment)
        {
            List<Affects> list = affectsList.Where(x => x.SecondAssignment == assignment).ToList();
            IStatementList statementList = ImplementationFactory.CreateStatementList();

            foreach (Affects affects in list)
                statementList.AddStatement(affects.FirstAssignment);

            return statementList;
        }

        public IStatementList GetAffectsT(Assign assignment)
        {
            return new RecursionContext(this).GetAffectsT(assignment);
        }

        public bool IsAffects(Assign assignment1, Assign assignment2)
        {
            IStatementList affectingAssignments = GetAffects(assignment2);
            return affectingAssignments.Contains(assignment1);
        }

        public bool IsAffectsT(Assign assignment1, Assign assignment2)
        {
            IStatementList affectingAssignments = GetAffectsT(assignment2);
            return affectingAssignments.Contains(assignment1);
        }

        public void SetAffects(Assign assignment1, Assign assignment2)
        {
            if (!IsAffects(assignment1, assignment2))
                affectsList.Add(new Affects(assignment1, assignment2));
        }

        private class RecursionContext
        {
            private IStatementList statementList;
            private AffectsTable outer;

            public RecursionContext(AffectsTable outer)
            {
                statementList = ImplementationFactory.CreateStatementList();
                this.outer = outer;
            }

            public IStatementList GetAffectedByT(Assign assignment)
            {
                IStatementList affectedList = outer.GetAffectedBy(assignment);
                if (affectedList != null)
                {
                    foreach (Statement affectedAssignment in affectedList)
                    {
                        GetAffectedByT(affectedAssignment as Assign);
                        statementList.AddStatement(affectedAssignment);
                    }
                }
                return statementList;
            }

            public IStatementList GetAffectsT(Assign assignment)
            {
                IStatementList affectingList = outer.GetAffects(assignment);
                if (affectingList != null)
                {
                    foreach (Statement affectingAssignment in affectingList)
                    {
                        GetAffectsT(affectingAssignment as Assign);
                        statementList.AddStatement(affectingAssignment);
                    }
                }
                return statementList;
            }
        }
    }
}
