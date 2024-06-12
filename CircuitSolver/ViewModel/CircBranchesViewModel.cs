using CircuitSolver.Model;
using CircuitSolver.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitSolver.ViewModel
{
    public partial class CircBranchesViewModel : ObservableObject
    {
        //[ObservableProperty]
        //ObservableCollection<Branch> branches;

        //public CircBranchesViewModel(List<Branch> branches)
        //{
        //    this.branches = [.. branches];
        //}

        //[RelayCommand]
        //public void Delete(Branch branch)
        //{
        //    if (branch == null) return;

        //    branches.Remove(branch);
        //}

        //[RelayCommand]
        //public async Task ViewDetails(Branch branch)
        //{
        //    if (branch == null) return;

        //    await Shell.Current.GoToAsync($"{nameof(BranchDetailsPage)}", true,
        //        new Dictionary<string, object>
        //        {
        //            {"Branch", branch }
        //        });
        //}
    }
}
