using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.Exceptions
{
    public class ParserException : Exception
    {
        public int Line { get; set; }
        public int Row { get; set; }
        public ParserException(string msg, int line, int row) : base(msg)
        {
            Line = line;
            Row = row;
        }
    }
}
