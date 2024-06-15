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
            Routing.RegisterRoute(nameof(NodeDetailsPage), typeof(NodeDetailsPage));
            Routing.RegisterRoute(nameof(BranchDetailsPage), typeof(BranchDetailsPage));
            Routing.RegisterRoute(nameof(CircBranchesPage), typeof(CircBranchesPage));
            Routing.RegisterRoute(nameof(CircNodesPage), typeof(CircBranchesPage));
            Routing.RegisterRoute(nameof(CircDiagramPage), typeof(CircDiagramPage));
            Routing.RegisterRoute(nameof(CircSolvePage), typeof(CircSolvePage));
            Routing.RegisterRoute(nameof(AddBranch1Page), typeof(AddBranch1Page));
            Routing.RegisterRoute(nameof(AddBranch2Page), typeof(AddBranch2Page));
            Routing.RegisterRoute(nameof(AddControlledSource3Page), typeof(AddControlledSource3Page));


        }
    }
}
