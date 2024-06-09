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

namespace CircuitSolver.ViewModel
{
    [QueryProperty("circuit", "circuit")]
    public partial class CircViewModel : ObservableObject
    {
        [ObservableProperty]
        public Circuit circuit;

        public CircViewModel(Circuit circuit) 
        {
            this.circuit = circuit; 
        }

    }
}
