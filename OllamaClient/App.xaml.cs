using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ollama.Data.Entites;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OllamaClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            services.AddDbContext<OllamaClientContext>(options => 
                options.UseSqlServer("Server=.;Database=OllamaDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }

}
