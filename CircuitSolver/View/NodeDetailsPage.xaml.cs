using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class NodeDetailsPage : ContentPage
{
	public NodeDetailsPage(NodeDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}