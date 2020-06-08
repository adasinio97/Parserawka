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
        private BindingsManager bindingsManager;

        public PqlTupleOutput(BindingsManager bindingsManager)
        {
            Declarations = ImplementationFactory.CreateDeclarationList();
            this.bindingsManager = bindingsManager;
        }

        public override string ToString()
        {
            if (Declarations.GetSize() > 0)
            {
                string result = "";
                string separator = "";
                List<string> tuples = new List<string>();
                List<BindingNode> firstNodes = bindingsManager.GetListNodes(Declarations[0].EntityList);
                BuildTuples(tuples, "", firstNodes, 0);
                foreach (string tuple in tuples)
                {
                    result += separator;
                    result += tuple;
                    separator = ",";
                }
                result = result.TrimStart();
                return result;
            }
            return "none";
        }

        private void BuildTuples(List<string> tuples, string tuple, List<BindingNode> nodes, int declarationIndex)
        {
            if (declarationIndex < Declarations.GetSize())
            {
                foreach (BindingNode node in nodes)
                {
                    string newTuple = string.Copy(tuple);
                    PqlDeclaration currentDeclaration = Declarations[declarationIndex];
                    string attribute = currentDeclaration.IsSecondaryAttribute ? node.Entity.SecondaryAttribute.AttributeValue : node.Entity.Attribute.AttributeValue;

                    newTuple += " " + attribute;
                    List<BindingNode> nextNodes = null;
                    if (declarationIndex + 1 < Declarations.GetSize())
                    {
                        PqlDeclaration nextDeclaration = Declarations[declarationIndex + 1];
                        nextNodes = bindingsManager.GetBoundNodes(node, nextDeclaration.EntityList);
                    }
                    BuildTuples(tuples, newTuple, nextNodes, declarationIndex + 1);
                }
                if (nodes.Count == 0)
                    tuples.Add(tuple);
            }
            else
                tuples.Add(tuple);
        }
    }
}
