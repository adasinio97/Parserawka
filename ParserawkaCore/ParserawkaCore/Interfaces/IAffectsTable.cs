using ParserawkaCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface IAffectsTable
    {
        void SetAffects(Assign assignment1, Assign assignment2);
        IStatementList GetAffects(Assign assignment);
        IStatementList GetAffectsT(Assign assignment);
        IStatementList GetAffectedBy(Assign assignment);
        IStatementList GetAffectedByT(Assign assignment);
        bool IsAffects(Assign assignment1, Assign assignment2);
        bool IsAffectsT(Assign assignment1, Assign assignment2);
    }
}
