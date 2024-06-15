using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.Model
{
    public class Branch : INotifyPropertyChanged
    {
        public int id {  get; set; }

        public float v { get; set; }
        public float i { get; set; }
        public float r { get; set; }

        public Node nNode { get; set; }
        public Node pNode { get; set; }

        public string desc { get; set; }
        public string briefDesc { get; set; }
        public string type { get; protected set; }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
