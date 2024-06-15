using CircuitSolver.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitSolver.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using WinRT;

namespace CircuitSolver.ViewModel
{
    //[QueryProperty("Id", "Id")]
    public partial class AddBranchViewModel : ObservableObject
    {
        private byte curPage = 1;

        [ObservableProperty]
        Branch branch;
        [ObservableProperty]
        string name;
        [ObservableProperty]
        int id = 0;
        [ObservableProperty]
        string idString = "ID: B";
        [ObservableProperty]
        string entry;

        [ObservableProperty]
        string nNodePrompt;
        [ObservableProperty]
        string pNodePrompt;
        [ObservableProperty]
        string valPrompt;

        //For Controlled Element
        [ObservableProperty]
        string contPNodePrompt;
        [ObservableProperty]
        Node contPNode;
        [ObservableProperty]
        string verification;
        object dirContSource;
        [ObservableProperty]
        string dirVerification;
        short contPolarity;

        [ObservableProperty]
        ObservableCollection<Node> nodes;
        [ObservableProperty]
        ObservableCollection<Branch> branches;

        [ObservableProperty]
        Branch controlBranch;
        [ObservableProperty]
        Node nNode;
        [ObservableProperty]
        Node pNode;

        [ObservableProperty]
        string valString;

        float val;
        [ObservableProperty]
        bool controlledSource;
        private bool currentSource;
        private bool voltageSource;

        public object Selection { get; set; }

        public object DirContSource
        {
            get => dirContSource;
            set
            {
                dirContSource = value;
                OnPropertyChanged(nameof(dirContSource));
                contPolarity = Byte.Parse(DirContSource.ToString());

                if (DirContSource.ToString().Equals("-1"))
                {
                    if (Selection.ToString().Equals("3") || Selection.ToString().Equals("4"))
                    {
                        DirVerification = "Controlling current is defined to flow from " + ControlBranch.pNode.name + " to " + ControlBranch.nNode.name;
                    }
                    else
                    {
                        DirVerification = "Controlling voltage is defined to be higher at " + ControlBranch.nNode.name + " and lower at " + ControlBranch.pNode.name;
                    }
                }
                else
                {

                    if (Selection.ToString().Equals("3") || Selection.ToString().Equals("4"))
                    {
                        DirVerification = "Controlling current is defined to flow from " + ControlBranch.pNode.name + " to " + ControlBranch.nNode.name;
                    }
                    else
                    {
                        DirVerification = "Controlling voltage is defined to be higher at " + ControlBranch.nNode.name + " and lower at " + ControlBranch.pNode.name;
                    }
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddBranchViewModel()
        {
            curPage = 1;
            IdString = "ID: B" + id;
            controlledSource = false;
            dirVerification = "";
        }

        
        [RelayCommand]
        public async Task Cancel()
        {
            try
            {
                await Shell.Current.DisplayAlert("Cancel", curPage + "Cancel" + Id, "OK");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Cancel", ex.Message, "OK");
            }

            //if (curPage == 1)
            //{
            //    await Shell.Current.GoToAsync("..");
            //}
            //else if (curPage == 2)
            //{
            //    await Shell.Current.GoToAsync("../..");
            //}
            //else
            //{
            //    await Shell.Current.GoToAsync("../../..");
            //}
        }

        [RelayCommand]
        public async Task Back()
        {
            curPage--;
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task Next()
        {
            curPage++;
            
            if (curPage == 2)
            {
                switch (Selection.ToString())
                {
                    case "3":
                        //Current and Controlled
                        currentSource = true;
                        ControlledSource = true;
                        voltageSource = false;
                        ContPNodePrompt = "Select where current flows towards for controlling element: ";
                        break;
                    case "5":
                        //Current and Controlled
                        currentSource = true;
                        ControlledSource = true;
                        voltageSource = false;
                        ContPNodePrompt = "Select the higher voltage node for controlling element: ";
                        break;
                    case "4":
                        //Voltage and Controlled 
                        currentSource = false;
                        ControlledSource = true;
                        voltageSource = true;
                        ContPNodePrompt = "Select where current flows towards for controlling element: ";
                        break;
                    case "6":
                        //Voltage and Controlled 
                        currentSource = false;
                        ControlledSource = true;
                        voltageSource = true;
                        ContPNodePrompt = "Select the higher voltage node for controlling element: ";
                        break;
                    case "1":
                        //Current
                        currentSource = true;
                        ControlledSource = false;
                        voltageSource = false;
                        break;
                    case "2":
                        //Voltage
                        currentSource = false;
                        ControlledSource = false;
                        voltageSource = true;
                        break;
                    case "0":
                    default:
                        //Resistor
                        currentSource = false;
                        ControlledSource = false;
                        voltageSource = false;
                        break;
                }

                if (currentSource)
                {
                    NNodePrompt = "Select where current flows from: ";
                    PNodePrompt = "Select where current flows towards: ";
                }
                else if (voltageSource)
                {
                    NNodePrompt = "Select the node of lower voltage: ";
                    PNodePrompt = "Select the node of higher voltage: ";
                }
                else
                {
                    NNodePrompt = "Select the first node: ";
                    PNodePrompt = "Select the second node: ";
                }

                await Shell.Current.GoToAsync($"{nameof(AddBranch2Page)}");
            }
            else if (curPage == 3 && ControlledSource)
            {
                if (ControlledSource)
                {
                    await Shell.Current.GoToAsync($"{nameof(AddControlledSource3Page)}");
                }
                else
                {
                    Create();
                    await Shell.Current.GoToAsync("../..");
                }
            }
            else
            {
                Create();
                await Shell.Current.GoToAsync("../../..");
            }
        }


        [RelayCommand]
        public void Create()
        {
            switch (Selection.ToString())
            {
                case "3":
                    Branch = new CCCS(Id, contPolarity * val, ControlBranch, NNode, PNode);
                    break;
                case "5":
                    Branch = new VCCS(Id, contPolarity * val, ControlBranch, NNode, PNode);
                    break;
                case "4":
                    Branch = new CCVS(Id, contPolarity * val, ControlBranch, NNode, PNode);
                    break;
                case "6":
                    Branch = new VCVS(Id, contPolarity * val, ControlBranch, NNode, PNode);
                    break;
                case "1":
                    Branch = new ISource(Id, val, NNode, PNode);
                    break;
                case "2":
                    Branch = new VSource(Id, val, NNode, PNode);
                    break;
                case "0":
                default:
                    Branch = new Resistor(Id, val, NNode, PNode);
                    break;
            }

            Branches.Add(Branch);
        }
    }
}
