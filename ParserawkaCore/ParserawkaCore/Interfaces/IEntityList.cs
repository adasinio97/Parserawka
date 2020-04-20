using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface IEntityList : IEnumerable<IEntity>
    {
        IEntity this[int i] { get; }
        int AddEntity(IEntity entity);
        IEntity GetEntityByIndex(int index);
        IEntity GetEntityByAttribute(string attributeValue);
        int GetIndexByAttribute(string attributeValue);
        int GetSize();
        bool Contains(IEntity entity);
        bool Contains(string attributeValue);

        IEntityList Copy();
        IEntityList Intersection(IEntityList otherEntityList);
        IEntityList Sum(IEntityList otherEntityList);
        IEntityList FilterByType(Type type);
        IEntityList FilterByAttribute(string attributeValue);
    }
}
