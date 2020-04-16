using ParserawkaCore.Parser.AstElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Interfaces
{
    public interface IParser
    {
        AST Parse();
    }
}
