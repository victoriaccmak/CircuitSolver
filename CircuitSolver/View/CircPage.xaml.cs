using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class CircPage : ContentPage
{
	public CircPage(CircViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}