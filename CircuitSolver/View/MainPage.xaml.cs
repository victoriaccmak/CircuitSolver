using CircuitSolver.Model;
using CircuitSolver.ViewModel;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace CircuitSolver.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }

}
