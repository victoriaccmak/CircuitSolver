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

        public List<Node> nodes;
        public List<Branch> branches;

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
            nodes = new List<Node>();
            branches = new List<Branch>();
            DateAsString = DateTime.ToString("f");
            isFave = false;
            FaveImg = "starunfilledimg.png";
            TypeAsString = TYPES[type];
        }

        public void AddNode(string name)
        {
            Node temp = new Node(name, nodes.Count, nodes);

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].AddNewNode();
            }

            nodes.Add(temp);
            DisplayBranches();
        }

        public void DisplayBranches()
        {
            for (int i = 0; i < branches.Count; i++)
            {
                Console.WriteLine(branches[i].GetDesc());
            }
        }
        public void DisplayNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Display();
            }
        }

        public void CalcNodalVoltages(Node ground)
        {
            float[][] matrix = new float[nodes.Count][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new float[nodes.Count + 1];
            }

            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i] is Resistor)
                {
                    if (!branches[i].GetP().GetHasVSource())
                    {
                        matrix[branches[i].GetP().GetId()][branches[i].GetP().GetId()] += 1f / branches[i].GetR();
                        matrix[branches[i].GetP().GetId()][branches[i].GetN().GetId()] -= 1f / branches[i].GetR();
                    }

                    if (!branches[i].GetN().GetHasVSource())
                    {
                        matrix[branches[i].GetN().GetId()][branches[i].GetN().GetId()] += 1f / branches[i].GetR();
                        matrix[branches[i].GetN().GetId()][branches[i].GetP().GetId()] -= 1f / branches[i].GetR();
                    }
                }
                else if (branches[i] is ISource)
                {
                    if (!branches[i].GetP().GetHasVSource())
                    {
                        matrix[branches[i].GetP().GetId()][nodes.Count] += branches[i].GetI();
                    }

                    if (!branches[i].GetN().GetHasVSource())
                    {
                        matrix[branches[i].GetN().GetId()][nodes.Count] -= branches[i].GetI();
                    }
                }
                else if (branches[i] is VSource)
                {
                    if (branches[i].GetP().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetP().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetP().GetId()][branches[i].GetN().GetId()] = -1;
                        matrix[branches[i].GetP().GetId()][nodes.Count] = branches[i].GetV();
                    }
                    else if (branches[i].GetN().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetN().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetN().GetId()][branches[i].GetN().GetId()] = -1;
                        matrix[branches[i].GetN().GetId()][nodes.Count] = branches[i].GetV();
                    }
                }
                else if (branches[i] is VCVS)
                {
                    if (branches[i].GetP().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetP().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetP().GetId()][branches[i].GetN().GetId()] = -1;
                        matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult();
                        matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult();
                    }
                    else if (branches[i].GetN().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetN().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetN().GetId()][branches[i].GetN().GetId()] = -1;
                        matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult();
                        matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult();
                    }
                }
                else if (branches[i] is CCVS)
                {
                    if (branches[i].GetP().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetP().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetP().GetId()][branches[i].GetN().GetId()] = -1;

                        if (branches[i].GetControlBranch() is ISource)
                        {
                            matrix[branches[i].GetP().GetId()][nodes.Count] = branches[i].GetMult() * branches[i].GetControlBranch().GetI();
                        }
                        else if (branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                            matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                        }
                    }
                    else if (branches[i].GetN().GetVSourceNum() == branches[i].GetId())
                    {
                        matrix[branches[i].GetN().GetId()][branches[i].GetP().GetId()] = 1;
                        matrix[branches[i].GetN().GetId()][branches[i].GetN().GetId()] = -1;

                        if (branches[i].GetControlBranch() is ISource)
                        {
                            matrix[branches[i].GetN().GetId()][nodes.Count] = branches[i].GetMult() * branches[i].GetControlBranch().GetI();
                        }
                        else if (branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                            matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                        }
                    }
                }
                else if (branches[i] is VCCS)
                {
                    if (!branches[i].GetP().GetHasVSource())
                    {
                        matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult();
                        matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult();
                    }

                    if (!branches[i].GetN().GetHasVSource())
                    {
                        matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetP().GetId()] += branches[i].GetMult();
                        matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetN().GetId()] -= branches[i].GetMult();
                    }
                }
                else if (branches[i] is CCCS)
                {
                    if (!branches[i].GetP().GetHasVSource())
                    {
                        if (branches[i].GetControlBranch() is ISource)
                        {
                            matrix[branches[i].GetP().GetId()][nodes.Count] = branches[i].GetMult() * branches[i].GetControlBranch().GetI();
                        }
                        else if (branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetP().GetId()] -= branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                            matrix[branches[i].GetP().GetId()][branches[i].GetControlBranch().GetN().GetId()] += branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                        }
                    }

                    if (!branches[i].GetN().GetHasVSource())
                    {
                        if (branches[i].GetControlBranch() is ISource)
                        {
                            matrix[branches[i].GetN().GetId()][nodes.Count] = -branches[i].GetMult() * branches[i].GetControlBranch().GetI();
                        }
                        else if (branches[i].GetControlBranch() is Resistor)
                        {
                            matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetP().GetId()] += branches[i].GetMult() / branches[i].GetControlBranch().GetR();
                            matrix[branches[i].GetN().GetId()][branches[i].GetControlBranch().GetN().GetId()] -= branches[i].GetMult() / branches[i].GetControlBranch().GetR();
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
                        nodes[j].SetV(matrix[i][matrix[i].Length - 1]);
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
            for (int i = 0; i < nodes.Count; i++)
            {
                Console.Write(nodes[i].GetName().PadRight(10));
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
            for (int i = 0; i < branches.Count; i++)
            {
                branches[i].CalcI();
            }
        }

        public void RemoveBranch(int id)
        {
            branches.RemoveAt(id);

            for (int i = id; i < branches.Count; i++)
            {
                branches[i].SetId(i);
            }
        }

        public void RemoveNode(int id)
        {
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].GetP().GetId() == id || branches[i].GetN().GetId() == id)
                {
                    RemoveBranch(i);
                    i--;
                }
            }

            nodes.RemoveAt(id);

            //Shift ID
            for (int i = id; i < nodes.Count; i++)
            {
                nodes[i].SetId(i);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].UpdateDeletedNode(id);
            }
        }
    }
}
