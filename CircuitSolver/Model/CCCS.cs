using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class CCCS : Branch
    {
        private float multiplier;
        private Branch branch;

        public CCCS(int id, float multiplier, Branch branch, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.multiplier = multiplier;
            this.branch = branch;
            desc = id + " - CCCS - " + multiplier + " * i" + branch.GetId() + ", between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = multiplier + "I" + branch.GetId() + " CCCS";
        }

        override public Branch GetControlBranch()
        {
            return branch;
        }

        override public float GetMult()
        {
            return multiplier;
        }

        override public void CalcI()
        {
            i = multiplier * branch.GetI();
        }
    }
}
