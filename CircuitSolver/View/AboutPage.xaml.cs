using CircuitSolver.ViewModel;

namespace CircuitSolver.View;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}