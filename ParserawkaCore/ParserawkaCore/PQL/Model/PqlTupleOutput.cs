using ParserawkaCore.Interfaces;
using ParserawkaCore.PQL.AstElements;
using ParserawkaCore.PQL.Interfaces;
using ParserawkaCore.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParserawkaCore.PQL.Model
{
    public class PqlTupleOutput : PqlOutput
    {
        public IDeclarationList Declarations { get; set; }

        public PqlTupleOutput()
        {
            Declarations = ImplementationFactory.CreateDeclarationList();
        }

        public override string ToString()
        {
            string s = "";
            foreach (PqlDeclaration declaration in Declarations)
            {
                string separator = "";
                foreach (IEntity entity in declaration.EntityList)
                {
                    s += separator;
                    s += declaration.DesignEntity.Value.ToString();
                    s += " ";
                    s += entity.Attribute.AttributeValue.ToString();
                    separator = ", ";
                }
                s += "\n";
            }
            return s;
        }
    }
}
