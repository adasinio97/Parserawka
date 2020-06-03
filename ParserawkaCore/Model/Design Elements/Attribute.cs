using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.Model
{
    public class Attribute
    {
        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }

        public Attribute(string attributeName, string attributeValue)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }
    }
}
