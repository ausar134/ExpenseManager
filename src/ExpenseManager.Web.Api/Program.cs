#pragma warning disable CS0168 // Variable is declared but never used
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CA1031 // Do not catch general exception types
namespace ExpenseManager.Web.Api
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Threading.Tasks;

	using ExpenseManager.Models;
	using ExpenseManager.Persistence;

	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	using Serilog;
	using Serilog.Core;
	using Serilog.Events;
	using Serilog.Sinks.SystemConsole.Themes;

	public static class Program
	{
		private const string NoisyLogs =
			"SourceContext='Microsoft.Hosting.Lifetime' or SourceContext='Microsoft.EntityFrameworkCore.Database.Command' or SourceContext='Serilog.AspNetCore.RequestLoggingMiddleware'";

		private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch();

		public static async Task Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.BasicLoggerConfiguration()
				.CreateLogger();

			try
			{
				var host = CreateHostBuilder(args).Build();
				var configuration = host.Services.GetRequiredService<IConfiguration>();
				var environment = host.Services.GetRequiredService<IWebHostEnvironment>();

				Log.Logger = new LoggerConfiguration()
					.BasicLoggerConfiguration()
					.ActualLoggerConfiguration(configuration,environment)
					.CreateLogger();

				host.Run();
			}
			catch (Exception e)
			{
				Log.Fatal(e, LogMessages.UnhandledException, e.Message);
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSystemd()
				.UseWindowsService()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.ConfigureAppConfiguration((context,options)=>options
					.SetBasePath(context.HostingEnvironment.ContentRootPath)
					.AddJsonFile("appsettings.json",false,true)
					.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",true,true)
					.AddJsonFile("connectionStrings.json",true,true)
					.AddEnvironmentVariables())
				.ConfigureLogging(log => log.ClearProviders())
				.UseSerilog();

		private static LoggerConfiguration BasicLoggerConfiguration(this LoggerConfiguration logger)
			=> logger
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
				.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
				.MinimumLevel.Override("System", LogEventLevel.Error)
				.Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name)
				.Enrich.FromLogContext()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code)
				.WriteTo.Debug();

		private static LoggerConfiguration ActualLoggerConfiguration(
			this LoggerConfiguration logger,
			IConfiguration configuration,
			IHostEnvironment environment)
			=> logger
				.Enrich.WithProperty("Application", environment.ApplicationName)
				.Enrich.WithProperty("Environment", environment.EnvironmentName)
				.WriteTo.Logger(c => c
					.MinimumLevel.ControlledBy(LevelSwitch)
					.Filter.ByExcluding(NoisyLogs)
					.WriteTo.File(
						Path.Combine(Directory.GetCurrentDirectory(), "Logs", "ExpenseManager-.log"),
						fileSizeLimitBytes: 31457280,
						rollingInterval: RollingInterval.Day,
						rollOnFileSizeLimit: true,
						retainedFileCountLimit: 10,
						shared: true)
					.WriteTo.Seq(
						configuration["Seq:Uri"] ?? "https://orion.nessos.gr",
						apiKey: configuration["Seq:ApiKey"]));
	}
}
