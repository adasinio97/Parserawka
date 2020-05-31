using ParserawkaCore.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.Model
{
    public class AffectsTable : IAffectsTable
    {
        public IStatementList GetAffectedBy(Assign assignment)
        {
            if (assignment != null)
                return assignment.Affecting.Copy();
            else
                return ImplementationFactory.CreateStatementList();
        }

        public IStatementList GetAffectedByT(Assign assignment)
        {
            IStatementList affected = GetAffectedBy(assignment);
            for (int i = 0; i < affected.GetSize(); i++)
            {
                Assign affectedAssignment = affected[i] as Assign;
                affected.Sum(GetAffectedBy(affectedAssignment));
            }
            return affected;
        }

        public IStatementList GetAffects(Assign assignment)
        {
            if (assignment != null)
                return assignment.AffectedBy.Copy();
            else
                return ImplementationFactory.CreateStatementList();
        }

        public IStatementList GetAffectsT(Assign assignment)
        {
            IStatementList affecting = GetAffects(assignment);
            for (int i = 0; i < affecting.GetSize(); i++)
            {
                Assign affectingAssignment = affecting[i] as Assign;
                affecting.Sum(GetAffects(affectingAssignment));
            }
            return affecting;
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
            assignment1.Affecting.AddStatement(assignment2);
            assignment2.AffectedBy.AddStatement(assignment1);
        }
    }
}
