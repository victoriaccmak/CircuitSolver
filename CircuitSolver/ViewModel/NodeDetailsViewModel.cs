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
    [QueryProperty("Node", "Node")]
    public partial class NodeDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        Node node;

        public NodeDetailsViewModel()
        {
            
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
