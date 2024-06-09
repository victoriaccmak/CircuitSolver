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
            builder.Services.AddTransient<NewCircPage>();
            builder.Services.AddTransient<NewCircViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            //builder.Services.AddSingleton<IFileSaver>

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
