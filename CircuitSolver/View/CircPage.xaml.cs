using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class CircPage : TabbedPage
{
	public CircPage(CircViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}