using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL;
using ParserawkaCore.PQL.Model;
using ParserawkaCore.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserawkaCore.Model
{
    public class EntityList : IEntityList, IEnumerable<IEntity>
    {
        protected List<IEntity> list;
        private SortedDictionary<string, IEntity> dictionary;

        public string ListName { get; set; }

        public EntityList()
        {
            list = new List<IEntity>();
            dictionary = new SortedDictionary<string, IEntity>();
        }

        public IEntity this[int i] { get { return GetEntityByIndex(i); } }

        public int AddEntity(IEntity entity)
        {
            if (entity == null)
                return -1;
            int index;
            IEntity existingEntity = GetEntityByAttribute(entity.Attribute.AttributeValue.ToString());
            if (existingEntity != null)
                index = GetIndex(existingEntity);
            else
            {
                index = list.Count;
                list.Add(entity);
                dictionary.Add(entity.Attribute.AttributeValue.ToString(), entity);
            }
            return index;
        }

        public void RemoveEntity(IEntity entity)
        {
            list.Remove(entity);
            dictionary.Remove(entity.Attribute.AttributeValue.ToString());
        }

        public int GetIndex(IEntity entity)
        {
            return list.IndexOf(entity);
        }

        public int GetIndexByAttribute(string attributeValue)
        {
            return GetIndex(GetEntityByAttribute(attributeValue));
        }

        public int GetSize()
        {
            return list.Count;
        }

        public IEntity GetEntityByIndex(int index)
        {
            return list[index];
        }

        public IEntity GetEntityByAttribute(string attributeValue)
        {
            IEntity entity = null;
            dictionary.TryGetValue(attributeValue, out entity);
            return entity;
        }

        public bool Contains(IEntity entity)
        {
            return dictionary.ContainsKey(entity.Attribute.AttributeValue.ToString());
        }

        public bool Contains(string attributeValue)
        {
            return dictionary.ContainsKey(attributeValue);
        }

        public IEntityList Copy()
        {
            EntityList copy = CreateNewList() as EntityList;
            foreach (IEntity entity in list)
            {
                copy.list.Add(entity);
                copy.dictionary.Add(entity.Attribute.AttributeValue.ToString(), entity);
            }
            return copy;
        }

        public IEntityList Intersection(IEntityList otherEntityList)
        {
            EntityList entityList = otherEntityList as EntityList;
            for (int i = 0; i < list.Count; i++)
            {
                IEntity entity = GetEntityByIndex(i);
                if (!entityList.Contains(entity))
                {
                    RemoveEntity(entity);
                    i--;
                }
            }
            return this;
        }

        public IEntityList Intersection(IEntityList otherEntityList, BindingsManager bindingsManager)
        {
            EntityList entityList = otherEntityList as EntityList;
            for (int i = 0; i < list.Count; i++)
            {
                IEntity entity = GetEntityByIndex(i);
                if (!entityList.Contains(entity))
                {
                    bindingsManager.RemoveBoundEntity(entity, this);
                    i--;
                }
            }
            return this;
        }

        public IEntityList Sum(IEntityList otherEntityList)
        {
            EntityList entityList = otherEntityList as EntityList;
            foreach (IEntity entity in entityList)
                AddEntity(entity);
            return this;
        }

        public IEntityList FilterByType(Type type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                IEntity entity = GetEntityByIndex(i);
                if (!type.IsAssignableFrom(entity.GetType()))
                {
                    RemoveEntity(entity);
                    i--;
                }
            }
            return this;
        }

        public IEntityList FilterByAttribute(string attributeValue)
        {
            IEntity entity = GetEntityByAttribute(attributeValue);
            list.Clear();
            dictionary.Clear();
            AddEntity(entity);
            return this;
        }

        public IEntityList FilterByAttributes(List<string> attributeValues)
        {
            for (int i = 0; i < list.Count; i++)
            {
                IEntity entity = GetEntityByIndex(i);
                if (!attributeValues.Contains(entity.Attribute.AttributeValue))
                {
                    RemoveEntity(entity);
                    i--;
                }
            }
            return this;
        }

        public virtual IEntityList CreateNewList()
        {
            return ImplementationFactory.CreateEntityList();
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public override string ToString()
        {
            return ListName;
        }
    }
}
