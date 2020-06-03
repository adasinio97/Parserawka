using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Parser.Exceptions
{
    public class TreeBuildingException : ParserException
    {
        public TreeBuildingException(string msg, int line, int row) : base(msg, line, row)
        {
        }
    }
}
