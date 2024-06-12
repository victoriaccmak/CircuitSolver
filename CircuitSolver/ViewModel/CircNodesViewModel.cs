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
    public partial class CircNodesViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Node> nodes;

        public CircNodesViewModel()
        {
        }

        [RelayCommand]
        public void Delete(Node node)
        {
            if (node == null) return;

            nodes.Remove(node);
        }

        [RelayCommand]
        public async Task ViewDetails(Node node)
        {
            if (node == null) return;

            await Shell.Current.GoToAsync($"{nameof(NodeDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Node", node }
                });
        }
    }

}
