using Microsoft.Extensions.Configuration;
using System.Reflection;
using TempoWorklogger.Contract.Config;
using TempoWorklogger.Contract.Services;
using TempoWorklogger.CQRS;
using TempoWorklogger.Dto.Config;
using TempoWorklogger.Service;
using TempoWorklogger.UI;

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
			.GetManifestResourceStream("TempoWorklogger.tempo-worklogger.appsettings.json");
        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
        builder.Configuration.AddConfiguration(config);
        
		builder.Services.AddSingleton<IAppConfig>(s =>
		{
			var appConfig = new AppConfig();
			builder.Configuration.GetSection("App").Bind(appConfig);
			appConfig.DatabaseFileName = Path.Combine(FileSystem.AppDataDirectory, appConfig.DatabaseFileName);
			return appConfig;
		});

        builder.Services.AddScoped<ITempoServiceFactory, TempoServiceFactory>();
        builder.Services.AddScoped<IFileReaderService, FileReaderService>();

        builder.Services.AddSingleton<IDbService>(options => {
			var appConfig = options.GetService<IAppConfig>();
			var dbService = new DbService(appConfig.DatabaseFileName);
			var dbContext = dbService.GetConnection(default, updateSchema: true).Result;

			return dbService;
        });

        builder.Services.AddCQRS();

		builder.Services.AddUIServices();

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        return builder.Build();
	}
}
