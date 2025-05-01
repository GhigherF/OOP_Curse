using CURSE.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;

namespace CURSE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Register dbContext
            services.AddDbContext<dbContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));



            // Register MainViewModel and any other services that depend on dbContext
            services.AddSingleton<MainViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            // Resolve the MainViewModel and set it as the DataContext for the MainWindow
            var mainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()

            };
           
            base.OnStartup(e);
            mainWindow.Show();

        }

    }
}
