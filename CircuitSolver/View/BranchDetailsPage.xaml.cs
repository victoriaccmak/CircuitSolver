using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class BranchDetailsPage : ContentPage
{
	public BranchDetailsPage(BranchDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}