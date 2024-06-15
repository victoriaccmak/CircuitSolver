using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class AddBranch1Page : ContentPage
{
	public AddBranch1Page(AddBranchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}