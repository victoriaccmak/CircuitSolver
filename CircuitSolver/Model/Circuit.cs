using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CircuitSolver.Model
{
    public class Circuit : INotifyPropertyChanged
    {
        public static readonly string[] TYPES = { "DC", "AC", "First Order", "Favorites" };

        public List<Node> Nodes { get; set; }
        public List<Branch> Branches { get; set; }

        public string Name { get; set; }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public DateTime DateTime {  get; set; }
        public string DateAsString {  get; }
        public byte Type {  get; set; }
        public string TypeAsString { get; private set; }
        public string FaveImg { get; private set; }

        bool isFave;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsFave
        {
            get => isFave;

            set
            {
                isFave = value;

                if (value)
                {
                    FaveImg = "starfilledimg.png";
                }
                else
                {
                    FaveImg = "starunfilledimg.png";
                }

                OnPropertyChanged(nameof(IsFave));
                OnPropertyChanged(nameof(FaveImg));
            }
        }

        public Circuit(string name, DateTime dateAndTime, byte type) 
        {
            Name = name;
            DateTime = dateAndTime; 
            Type = type;
            Nodes = new List<Node>();
            Branches = new List<Branch>();
            DateAsString = DateTime.ToString("f");
            isFave = false;
            FaveImg = "starunfilledimg.png";
            TypeAsString = TYPES[type];
        }

        public void AddNode(string name)
        {
            Node temp = new Node(name, Nodes.Count, Nodes);

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].AddNewNode();
            }

            Nodes.Add(temp);
            DisplayBranches();
        }

        public void DisplayBranches()
        {
            for (int i = 0; i < Branches.Count; i++)
            {
                Console.WriteLine(Branches[i].GetDesc());
            }
        }
        public void DisplayNodes()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].Display();
            }
        }

        public void CalcNodalVoltages(Node ground)
        {
            float[][] matrix = new float[Nodes.Count][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new float[Nodes.Count + 1];
            }

            for (int i = 0; i < Branches.Count; i++)
            {
                if (Branches[i] is Resistor)
                {
                    if (!Branches[i].GetP().GetHasVSource())
                    {
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetP().GetId()] += 1f / Branches[i].GetR();
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetN().GetId()] -= 1f / Branches[i].GetR();
                    }

                    if (!Branches[i].GetN().GetHasVSource())
                    {
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetN().GetId()] += 1f / Branches[i].GetR();
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetP().GetId()] -= 1f / Branches[i].GetR();
                    }
                }
                else if (Branches[i] is ISource)
                {
                    if (!Branches[i].GetP().GetHasVSource())
                    {
                        matrix[Branches[i].GetP().GetId()][Nodes.Count] += Branches[i].GetI();
                    }

                    if (!Branches[i].GetN().GetHasVSource())
                    {
                        matrix[Branches[i].GetN().GetId()][Nodes.Count] -= Branches[i].GetI();
                    }
                }
                else if (Branches[i] is VSource)
                {
                    if (Branches[i].GetP().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetN().GetId()] = -1;
                        matrix[Branches[i].GetP().GetId()][Nodes.Count] = Branches[i].GetV();
                    }
                    else if (Branches[i].GetN().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetN().GetId()] = -1;
                        matrix[Branches[i].GetN().GetId()][Nodes.Count] = Branches[i].GetV();
                    }
                }
                else if (Branches[i] is VCVS)
                {
                    if (Branches[i].GetP().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetN().GetId()] = -1;
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult();
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult();
                    }
                    else if (Branches[i].GetN().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetN().GetId()] = -1;
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult();
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult();
                    }
                }
                else if (Branches[i] is CCVS)
                {
                    if (Branches[i].GetP().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetN().GetId()] = -1;

                        if (Branches[i].GetControlBranch() is ISource)
                        {
                            matrix[Branches[i].GetP().GetId()][Nodes.Count] = Branches[i].GetMult() * Branches[i].GetControlBranch().GetI();
                        }
                        else if (Branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                            matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                        }
                    }
                    else if (Branches[i].GetN().GetVSourceNum() == Branches[i].GetId())
                    {
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetP().GetId()] = 1;
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetN().GetId()] = -1;

                        if (Branches[i].GetControlBranch() is ISource)
                        {
                            matrix[Branches[i].GetN().GetId()][Nodes.Count] = Branches[i].GetMult() * Branches[i].GetControlBranch().GetI();
                        }
                        else if (Branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                            matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                        }
                    }
                }
                else if (Branches[i] is VCCS)
                {
                    if (!Branches[i].GetP().GetHasVSource())
                    {
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult();
                        matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult();
                    }

                    if (!Branches[i].GetN().GetHasVSource())
                    {
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetP().GetId()] += Branches[i].GetMult();
                        matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetN().GetId()] -= Branches[i].GetMult();
                    }
                }
                else if (Branches[i] is CCCS)
                {
                    if (!Branches[i].GetP().GetHasVSource())
                    {
                        if (Branches[i].GetControlBranch() is ISource)
                        {
                            matrix[Branches[i].GetP().GetId()][Nodes.Count] = Branches[i].GetMult() * Branches[i].GetControlBranch().GetI();
                        }
                        else if (Branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetP().GetId()] -= Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                            matrix[Branches[i].GetP().GetId()][Branches[i].GetControlBranch().GetN().GetId()] += Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                        }
                    }

                    if (!Branches[i].GetN().GetHasVSource())
                    {
                        if (Branches[i].GetControlBranch() is ISource)
                        {
                            matrix[Branches[i].GetN().GetId()][Nodes.Count] = -Branches[i].GetMult() * Branches[i].GetControlBranch().GetI();
                        }
                        else if (Branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetP().GetId()] += Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                            matrix[Branches[i].GetN().GetId()][Branches[i].GetControlBranch().GetN().GetId()] -= Branches[i].GetMult() / Branches[i].GetControlBranch().GetR();
                        }
                    }
                }
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i][ground.GetId()] = 0;
            }

            DisplayMatrix(matrix);
            Console.WriteLine("This is your matrix. Press ENTER to simplify.");
            Console.ReadLine();

            matrix = SolveMatrix(matrix, ground.GetId());
            ground.SetV(0);

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length - 1; j++)
                {
                    if (matrix[i][j] != 0)
                    {
                        Nodes[j].SetV(matrix[i][matrix[i].Length - 1]);
                        break;
                    }
                }
            }
        }

        //Has Issues
        public float[][] SolveMatrix(float[][] matrix, int gndId)
        {
            if (matrix.Length > 0)
            {
                for (int i = 0; i < matrix[0].Length - 1; i++)
                {
                    bool zeroCol = true;
                    int row = 0;
                    for (; row < matrix.Length; row++)
                    {
                        if (matrix[row][i] != 0)
                        {
                            bool leadingVal = true;

                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (matrix[row][j] != 0)
                                {
                                    leadingVal = false;
                                }
                            }

                            if (leadingVal)
                            {
                                zeroCol = false;
                                break;
                            }
                        }
                    }

                    if (!zeroCol)
                    {
                        Console.WriteLine("row: " + row);

                        //Multiply the row by the reciprocal of the ith, ith entry to make the ith, ith entry equal to 1
                        float recip = 1f / matrix[row][i];

                        Console.WriteLine("Multiply row " + row + " by " + Math.Round(recip, 4));

                        for (int j = 0; j < matrix[i].Length; j++)
                        {
                            matrix[row][j] *= recip;
                        }

                        DisplayMatrix(matrix);
                        Console.WriteLine();

                        //Make all other entries in the ith column equal to 0
                        for (int j = 0; j < matrix.Length; j++)
                        {
                            if (j != row)
                            {
                                float mult = matrix[j][i];
                                Console.WriteLine("Subtract " + mult + " R" + row + " from row " + j);

                                for (int k = 0; k < matrix[j].Length; k++)
                                {
                                    matrix[j][k] = matrix[j][k] - matrix[row][k] * mult;
                                }

                                DisplayMatrix(matrix);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }

            return matrix;
        }

        private void DisplayMatrix(float[][] matrix)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Console.Write(Nodes[i].GetName().PadRight(10));
            }

            Console.WriteLine();

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(Math.Round(matrix[i][j], 2).ToString().PadRight(10));
                }

                Console.WriteLine();
            }
        }

        public void CalcCurrents()
        {
            for (int i = 0; i < Branches.Count; i++)
            {
                Branches[i].CalcI();
            }
        }

        public void RemoveBranch(int id)
        {
            Branches.RemoveAt(id);

            for (int i = id; i < Branches.Count; i++)
            {
                Branches[i].SetId(i);
            }
        }

        public void RemoveNode(int id)
        {
            for (int i = 0; i < Branches.Count; i++)
            {
                if (Branches[i].GetP().GetId() == id || Branches[i].GetN().GetId() == id)
                {
                    RemoveBranch(i);
                    i--;
                }
            }

            Nodes.RemoveAt(id);

            //Shift ID
            for (int i = id; i < Nodes.Count; i++)
            {
                Nodes[i].SetId(i);
            }

            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].UpdateDeletedNode(id);
            }
        }
    }
}
