using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Makes sure that any dependencies MainPage needs can be injected by the DI container when page is created.
        builder.Services.AddSingleton<MainPage>(); 
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeysViewModel>();

		return builder.Build();
	}
}
