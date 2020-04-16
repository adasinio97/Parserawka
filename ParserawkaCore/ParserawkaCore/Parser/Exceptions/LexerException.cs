using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaCore.Parser.Exceptions
{
    public class LexerException : ParserException
    {
        public LexerException(string message, int line, int row) : base(message, line, row) { }
    }
}
