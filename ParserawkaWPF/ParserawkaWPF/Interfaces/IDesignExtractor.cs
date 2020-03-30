﻿using ParserawkaWPF.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Interfaces
{
    public interface IDesignExtractor
    {
        IVariableList Variables { get; }
        IStatementList Statements { get; }
        IFollowsTable FollowsTable { get; }
        IParentTable ParentTable { get; }
        IModifiesTable ModifiesTable { get; }
        IUsesTable UsesTable { get; }
        void ExtractData(AST root);
    }
}