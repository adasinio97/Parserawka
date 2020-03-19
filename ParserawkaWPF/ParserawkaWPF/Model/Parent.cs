using ParserawkaWPF.Model;

namespace ParserawkaWPF.PKB.Model
{
    class Parent
    {
        public Statement ParentStatement { get; set; }
        public Statement ChildStatement { get; set; }
        public Parent() { }
        public Parent(Statement parent, Statement child)
        {
            this.ParentStatement = parent;
            this.ChildStatement = child;
        }

    }
}
