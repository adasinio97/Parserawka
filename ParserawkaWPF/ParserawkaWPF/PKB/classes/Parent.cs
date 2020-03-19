using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.PKB.classes
{
    class Parent
    {
        public Statement parent { get; set; }
        public Statement child { get; set; }
        public Parent() { }
        public Parent(Statement parent, Statement child)
        {
            this.parent = parent;
            this.child = child;
        }
        
    }
}
