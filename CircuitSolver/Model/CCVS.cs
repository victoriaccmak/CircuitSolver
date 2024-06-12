using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class CCVS : Branch
    {
        private float multiplier;
        private Branch branch;

        public CCVS(int id, float multiplier, Branch branch, Node nNode, Node pNode) : base(id, nNode, pNode)
        {
            this.multiplier = multiplier;
            this.branch = branch;
            desc = id + " - CCVS - " + multiplier + " * v" + branch.GetId() + ", between " + nNode.GetName() + " and " + pNode.GetName();
            briefDesc = multiplier + "I" + branch.GetId() + " CCVS";
            type = nameof(CCVS);
        }

        override public Branch GetControlBranch()
        {
            return branch;
        }

        override public float GetMult()
        {
            return multiplier;
        }

        virtual public void CalcI()
        {
            //
            i = float.NaN;
        }
    }
}
