using CircuitSolver.View;
using Microsoft.Maui;

namespace CircuitSolver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
