using CircuitSolver.View;
using CircuitSolver.ViewModel;

namespace CircuitSolver
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(CircPage), typeof(CircPage));
            Routing.RegisterRoute(nameof(NewCircPage), typeof(NewCircPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}
