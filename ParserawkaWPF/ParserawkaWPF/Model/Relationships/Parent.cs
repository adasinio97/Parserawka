namespace ParserawkaWPF.Model
{
    class Parent
    {
        public Statement ParentStatement { get; set; }
        public Statement ChildStatement { get; set; }

        public Parent(Statement parent, Statement child)
        {
            ParentStatement = parent;
            ChildStatement = child;
        }

    }
}
