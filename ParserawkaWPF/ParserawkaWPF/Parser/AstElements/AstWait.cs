using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserawkaWPF.Parser.AstElements
{
    class AstWait : AST
    {
        public AST Condition;
        public Token Time;

        public AstWait(AST condition, Token time)
        {
            Condition = condition;
            Time = time;
        }

        public override string ToString()
        {
            if(Condition != null)
            {
                return "wait " + Condition.ToString() + " : " + Time.Value.ToString() + "s";
            }
            else
            {
                return "wait " + Time.Value.ToString() + "s";
            }
        }
    }
}
