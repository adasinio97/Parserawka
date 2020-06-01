using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.PQL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.PQL
{
    public class BindingsManager
    {
        private List<BindingNode> looseNodes;

        public BindingsManager()
        {
            looseNodes = new List<BindingNode>();
        }

        public void CreateMultipleBindings(IEntity entity1, IEntityList sourceList, IEntityList list1, IEntityList list2)
        {
            BindingNode firstNode = GetNode(entity1, list1);
            if (firstNode == null)
            {
                firstNode = new BindingNode(entity1, list1);
                looseNodes.Add(firstNode);
            }

            foreach (IEntity entity2 in sourceList)
            {
                if (entity2 != null && list2.Contains(entity2))
                {
                    BindingNode secondNode = GetNode(entity2, list2);
                    if (secondNode == null)
                    {
                        secondNode = new BindingNode(entity2, list2);
                        looseNodes.Add(secondNode);
                    }
                    firstNode.Bind(secondNode);
                }
            }
        }

        public void CreateMultipleBindingsOneWay(IEntity entity1, IEntityList sourceList, IEntityList list1, IEntityList list2)
        {
            BindingNode firstNode = GetNode(entity1, list1);
            if (firstNode == null)
            {
                firstNode = new BindingNode(entity1, list1);
                looseNodes.Add(firstNode);
            }

            foreach (IEntity entity2 in sourceList)
            {
                if (entity2 != null && list2.Contains(entity2))
                {
                    BindingNode secondNode = GetNode(entity2, list2);
                    if (secondNode == null)
                    {
                        secondNode = new BindingNode(entity2, list2);
                        looseNodes.Add(secondNode);
                    }
                    firstNode.BindOneWay(secondNode);
                }
            }
        }

        public void CreateBinding(IEntity entity1, IEntity entity2, IEntityList list1, IEntityList list2)
        {
            BindingNode firstNode = GetNode(entity1, list1);
            if (firstNode == null)
            {
                firstNode = new BindingNode(entity1, list1);
                looseNodes.Add(firstNode);
            }
            BindingNode secondNode = GetNode(entity2, list2);
            if (secondNode == null)
            {
                secondNode = new BindingNode(entity2, list2);
                looseNodes.Add(secondNode);
            }
            firstNode.Bind(secondNode);
        }

        private BindingNode GetNode(IEntity entity, IEntityList entityList)
        {
            return looseNodes.Where(x => x.Entity == entity && x.EntityList == entityList).FirstOrDefault();
        }

        public void RemoveBoundEntity(IEntity entity, IEntityList entityList)
        {
            BindingNode node = GetNode(entity, entityList);
            if (node != null)
            {
                node.RemoveNode();
                looseNodes.Remove(node);
            }
            else
                entityList.RemoveEntity(entity);
        }
    }
}
