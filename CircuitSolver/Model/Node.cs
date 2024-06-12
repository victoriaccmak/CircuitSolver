using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class Node
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<List<Branch>> branches { get; set; }
        public float v { get; set; }
        public string desc = "";
        private List<List<string>> inDepthDesc { get; set; }
        private int vSourceNum { get; set; }
        private bool hasVSource { get; set; }

        public Node()
        {
        }

        public Node(string name, int id, List<Node> nodes)
        {
            this.name = name;
            this.id = id;
            branches = new List<List<Branch>>();
            vSourceNum = -1;
            hasVSource = false;
            inDepthDesc = new List<List<string>>();

            for (int i = 0; i <= nodes.Count; i++)
            {
                branches.Add(new List<Branch>());
                inDepthDesc.Add(new List<string>());
            }

            desc = "Name: " + name + " , ID: " + id;
        }

        public void SetV(float v)
        {
            this.v = v;
        }

        public string GetName()
        {
            return name;
        }

        public int GetId()
        {
            return id;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public float GetV()
        {
            return v;
        }

        public int GetVSourceNum()
        {
            return vSourceNum;
        }

        public bool GetHasVSource()
        {
            return hasVSource;
        }

        public void AddNewNode()
        {
            branches.Add(new List<Branch>());
            inDepthDesc.Add(new List<string>());
        }

        public void AddBranch(Branch branch)
        {
            int otherNodeId = branch.GetN().GetId();
            string line = "\tpositive/tip side node of " + branch.GetBriefDesc() + "(" + branch.GetId() + ") with node " + otherNodeId + "(" + branch.GetN().GetName() + ") as negative/tail side node";

            if (otherNodeId == id)
            {
                otherNodeId = branch.GetP().GetId();
                line = "\tnegative/tail side node of " + branch.GetBriefDesc() + "(" + branch.GetId() + ") with node " + otherNodeId + "(" + branch.GetP().GetName() + ") as positive/tip side node";
            }

            branches[otherNodeId].Add(branch);

            if (branch is VSource || branch is VCVS || branch is CCVS)
            {
                hasVSource = true;

                if (id < otherNodeId)
                {
                    vSourceNum = branch.GetId();
                }
            }

            inDepthDesc[otherNodeId].Add(line);
        }

        public void Display()
        {
            Console.WriteLine(desc);

            for (int i = 0; i < inDepthDesc.Count; i++)
            {
                for (int j = 0; j < inDepthDesc[i].Count; j++)
                {
                    Console.WriteLine(inDepthDesc[i][j]);
                }
            }
        }

        public bool IsConnected()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateDeletedNode(int deleteId)
        {
            desc = "Name: " + name + " , ID: " + id;
            branches.RemoveAt(deleteId);
            inDepthDesc.RemoveAt(deleteId);
        }
    }
}
