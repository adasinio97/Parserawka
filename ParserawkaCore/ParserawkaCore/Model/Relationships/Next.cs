using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class Next
    {
        public Statement FirstStatement { get; set; }
        public Statement SecondStatement { get; set; }

        public Next(Statement firstStatement, Statement secondStatement)
        {
            FirstStatement = firstStatement;
            SecondStatement = secondStatement;
        }
    }
}
