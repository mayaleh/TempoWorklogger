using Microsoft.Extensions.Configuration;
using System.Reflection;
using TempoWorklogger.Contract.Config;
using TempoWorklogger.Dto.Config;
using TempoWorklogger.Library;
using TempoWorklogger.States;

namespace TempoWorklogger;

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

		// Add appsettings
        using var stream = Assembly.GetExecutingAssembly()
			.GetManifestResourceStream("tempo-worklogger.appsettings.json");
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(config);
        
		builder.Services.AddSingleton<IAppConfig>(s =>
		{
			var appConfig = new AppConfig();
			builder.Configuration.GetSection("App").Bind(appConfig);
			appConfig.DatabaseFileName = Path.Combine(FileSystem.AppDataDirectory, appConfig.DatabaseFileName);
			return appConfig;
		});

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		builder.Services.AddSingleton<ImportState>();

		builder.Services.AddTempoWorkloggerLibrary();

        return builder.Build();
	}
}
