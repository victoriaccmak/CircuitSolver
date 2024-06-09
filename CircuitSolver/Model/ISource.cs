using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class ISource : Branch
    {
        public ISource(int id, float i, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.i = i;
            desc = id + " - I Source - " + i + "A, between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = i + "A independent current source";
        }
    }
}
