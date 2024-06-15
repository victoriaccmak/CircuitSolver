using Microsoft.Extensions.Logging;
using CircuitSolver.ViewModel;
using CircuitSolver.View;

namespace CircuitSolver
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddSingleton<AboutViewModel>();
            builder.Services.AddTransient<CircPage>();
            builder.Services.AddTransient<CircViewModel>();
            builder.Services.AddSingleton<NewCircPage>();
            builder.Services.AddSingleton<NewCircViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddTransient<NodeDetailsPage>();
            builder.Services.AddTransient<NodeDetailsViewModel>();
            builder.Services.AddTransient<BranchDetailsPage>();
            builder.Services.AddTransient<BranchDetailsViewModel>();
            builder.Services.AddTransient<AddBranch1Page>();
            builder.Services.AddTransient<AddBranch2Page>();
            builder.Services.AddTransient<AddControlledSource3Page>();
            builder.Services.AddTransient<AddBranchViewModel>();


            //builder.Services.AddSingleton<IFileSaver>

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
