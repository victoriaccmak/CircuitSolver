using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircuitSolver.Model;

namespace CircuitSolver.ViewModel
{
    [QueryProperty("Branch", "Branch")]
    public partial class BranchDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        Branch branch;

        public BranchDetailsViewModel()
        {
            
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
