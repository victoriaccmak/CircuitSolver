using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class VCCS : Branch
    {
        private float multiplier;
        private Branch branch;

        public VCCS(int id, float multiplier, Branch branch, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.multiplier = multiplier;
            this.branch = branch;
            desc = id + " - VCCS - " + multiplier + " * v" + branch.GetId() + ", between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = multiplier + "V" + branch.GetId() + " VCCS";
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
            i = multiplier * (branch.GetP().GetV() - branch.GetN().GetV());
        }
    }
}
