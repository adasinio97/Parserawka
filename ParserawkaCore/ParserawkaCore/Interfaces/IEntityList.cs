using ParserawkaCore.PQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface IEntityList : IEnumerable<IEntity>
    {
        IEntity this[int i] { get; }
        string ListName { get; set; }

        int AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
        IEntity GetEntityByIndex(int index);
        IEntity GetEntityByAttribute(string attributeValue);
        int GetIndexByAttribute(string attributeValue);
        int GetSize();
        bool Contains(IEntity entity);
        bool Contains(string attributeValue);

        void CreateBindings(IEntity bindingEntity, IEntityList entitySource, IEntityList bindingTarget);
        IEntityList Copy();
        IEntityList Intersection(IEntityList otherEntityList);
        IEntityList Intersection(IEntityList otherEntityList, BindingsManager bindingsManager);
        IEntityList Sum(IEntityList otherEntityList);
        IEntityList FilterByType(Type type);
        IEntityList FilterByAttribute(string attributeValue);
        IEntityList FilterByAttributes(List<string> attributeValues);
    }
}
