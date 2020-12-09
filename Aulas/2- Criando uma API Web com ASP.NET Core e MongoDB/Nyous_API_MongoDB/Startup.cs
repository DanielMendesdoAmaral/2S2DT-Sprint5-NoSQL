using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nyous_API_MongoDB.Contexts;
using Nyous_API_MongoDB.Interfaces.Repositories;
using Nyous_API_MongoDB.Repositories;

namespace Nyous_API_MongoDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Transfere informa��es do "appsettings.json" para o context. 
            services.Configure<NyousDatabaseSettings>(
                Configuration.GetSection(nameof(NyousDatabaseSettings))); //<- Refere-se � se��o NyousDatabaseSettings do "appsettings.json". Pega os valores e atribui ao Context, nos valores correspondentes.

            //Inje��o de depend�ncia.
            services.AddSingleton<INyousDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<NyousDatabaseSettings>>().Value);

            //Inje��o de depend�ncia para o repository.
            services.AddSingleton<IEventoRepository, EventoRepository>();

            services.AddSingleton<ICategoriaRepository, CategoriaRepository>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}