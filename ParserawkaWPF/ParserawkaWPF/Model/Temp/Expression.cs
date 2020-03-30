using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Model
{
    public class Expression
    {
        public Expression ChildExpression { get; set; }
        public Factor Factor { get; set; }
    }
}
