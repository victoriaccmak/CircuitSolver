using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class VSource : Branch
    {
        public VSource(int id, float v, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.v = v;
            desc = id + " - V Source - " + v + "V, between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = v + "V independent voltage source";
            type = "Voltage Source";
        }

        override public void CalcI()
        {
            //i = (pNode.GetV() - nNode.GetV()) / r;
            i = float.NaN;
        }
    }
}
