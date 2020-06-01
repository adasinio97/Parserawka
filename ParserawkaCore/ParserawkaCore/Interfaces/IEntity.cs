using ParserawkaCore.Model;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Interfaces
{
    public interface IEntity
    {
        Attribute Attribute { get; set; }
        Attribute SecondaryAttribute { get; set; }
    }
}
