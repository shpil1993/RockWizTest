using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RockWizTest.Db;
using RockWizTest.Helpers;
using RockWizTest.Services;
using RockWizTest.View;
using System;
using System.Windows;

namespace RockWizTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<IUIAService, UIAService>();
                    services.AddHttpClient<IWordPredictionService, WordPredictionService>(client =>
                    {
                        client.BaseAddress = new Uri("https://services.lingapps.dk/");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "MjAyMy0wMS0xNQ==.b2xla3NhbmRyLnJ5YmFrQGV4aXN0ZWsuY29t.NzNmNjFjZGEzY2VlYjg2ZTY2N2VjMzNjYTA2MDMyMTg=");
                    })
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
                    services.AddDbContext<PredictionDbContext>(context =>
                    {
                        context.UseSqlite("Data Source=Dictionary.db");
                    });
                    services.AddTransient<ICustomWordPredictionService, CustomWordPredictionService>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();

            base.OnExit(e);
        }
    }
}
