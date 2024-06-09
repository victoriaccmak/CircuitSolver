using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class Branch
    {
        protected int id;
        protected float v;
        protected float i;
        protected float r;

        protected Node nNode;
        protected Node pNode;

        protected string desc = "";
        protected string briefDesc = "";

        public Branch(int id, Node nNode, Node pNode)
        {
            this.nNode = nNode;
            this.pNode = pNode;
            this.id = id;
        }

        public int GetId()
        {
            return id;
        }

        public string GetDesc()
        {
            return desc;
        }

        public string GetBriefDesc()
        {
            return briefDesc;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public void SetV(float v)
        {
            this.v = v;
        }
        public void SetI(float i)
        {
            this.i = i;
        }
        public void SetR(float r)
        {
            this.r = r;
        }
        public float GetV()
        {
            return v;
        }
        public float GetI()
        {
            return i;
        }
        public float GetR()
        {
            return r;
        }
        public Node GetN()
        {
            return nNode;
        }
        public Node GetP()
        {
            return pNode;
        }

        virtual public Branch GetControlBranch()
        {
            return this;
        }

        virtual public float GetMult()
        {
            return 0;
        }

        virtual public void CalcI()
        {

        }
    }
}
