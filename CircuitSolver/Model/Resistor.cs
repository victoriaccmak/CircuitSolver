using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class Resistor : Branch
    {
        public Resistor(int id, float r, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.r = r;
            desc = id + " - Resistor - " + r + " Ohms, between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = r + " ohm resistor";
        }

        override public void CalcI()
        {
            i = (pNode.GetV() - nNode.GetV()) / r;
        }
    }
}
