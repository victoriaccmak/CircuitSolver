using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class NewCircPage : ContentPage
{
	public NewCircPage(NewCircViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}