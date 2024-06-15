//Author: Victoria Mak
//File Name: CircViewModel.cs
//Project Name: CircuitSolver
//Creation Date: May 20, 2024
//Modified Date: 
//Description: A cross platform app that will solve linear circuits

using CommunityToolkit.Mvvm.ComponentModel;
using CircuitSolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Handlers.Items;
using CircuitSolver.View;
using System.Xml.Linq;

namespace CircuitSolver.ViewModel
{
    [QueryProperty("Circuit", "Circuit")]
    public partial class CircViewModel : ObservableObject
    {
        [ObservableProperty]
        public Circuit circuit;

        public CircViewModel() 
        {
            Circuit = new Circuit("s", DateTime.Now, 1);
            //Circuit = circuit; 
        }

        [RelayCommand]
        public void DeleteBranch(Branch branch)
        {
            if (branch == null) return;

            Circuit.Branches.Remove(branch);
        }

        [RelayCommand]
        public async Task ViewBranchDetails(Branch branch)
        {
            if (branch == null) return;

            await Shell.Current.GoToAsync($"{nameof(BranchDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Branch", branch }
                });
        }

        [RelayCommand]
        public void DeleteNode(Node node)
        {
            if (node == null) return;

            Circuit.Nodes.Remove(node);
        }

        [RelayCommand]
        public async Task ViewNodeDetails(Node node)
        {
            if (node == null) return;

            await Shell.Current.GoToAsync($"{nameof(NodeDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Node", node }
                });
        }

        [RelayCommand]
        public async Task AddNewBranch()
        {
            try
            {

                await Shell.Current.GoToAsync($"{nameof(AddBranch1Page)}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
