using ParserawkaCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.Model
{
    public class BindingNode
    {
        public IEntity Entity { get; private set; }
        public IEntityList EntityList { get; private set; }
        
        public List<BindingNode> RightNodes { get; set; }
        public List<BindingNode> LeftNodes { get; set; }

        public bool OneWay { get; set; }

        public BindingNode(IEntity entity, IEntityList entityList)
        {
            Entity = entity;
            EntityList = entityList;
            RightNodes = new List<BindingNode>();
            LeftNodes = new List<BindingNode>();
            OneWay = false;
        }

        public void Bind(BindingNode nextNode)
        {
            if (!RightNodes.Contains(nextNode))
                RightNodes.Add(nextNode);
            if (!nextNode.LeftNodes.Contains(this))
                nextNode.LeftNodes.Add(this);
        }

        public void BindOneWay(BindingNode nextNode)
        {
            if (!LeftNodes.Contains(nextNode))
                LeftNodes.Add(nextNode);
            nextNode.OneWay = true;
        }

        public void RemoveNode()
        {
            foreach (BindingNode node in LeftNodes)
                node.RemoveLeftBindings(this);
            foreach (BindingNode node in RightNodes)
                node.RemoveRightBindings(this);
            EntityList.RemoveEntity(Entity);
        }

        public void RemoveLeftBindings(BindingNode node)
        {
            bool removed = RightNodes.Remove(node);
            if (RightNodes.Count == 0 && (removed || OneWay))
                RemoveNode();
        }

        public void RemoveRightBindings(BindingNode node)
        {
            bool removed = LeftNodes.Remove(node);
            if (LeftNodes.Count == 0)
                RemoveNode();
        }

        public bool IsBound(BindingNode otherNode)
        {
            return LeftNodes.Contains(otherNode) || RightNodes.Contains(otherNode);
        }

        public override string ToString()
        {
            return Entity.ToString() + " list: " + " " + EntityList.ToString();
        }
    }
}
