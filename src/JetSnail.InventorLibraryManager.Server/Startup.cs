using System.Reflection;
using Autofac;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.DataStore.EFCore;
using JetSnail.InventorLibraryManager.Server.Hubs;
//using JetSnail.InventorLibraryManager.Software.Inventor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JetSnail.InventorLibraryManager.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"),
                        x => { x.MigrationsAssembly("JetSnail.InventorLibraryManager.Data"); }));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(2, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo {Title = "JetSnail.InventorLibraryManager.Server", Version = "v1"});
            });

            services.AddOptions();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 注册PlugIn
            builder.RegisterAssemblyTypes(typeof(GroupRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(InventorService).Assembly)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces();

            // 注册UseCase
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("UseCase"))
                .AsImplementedInterfaces();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("./v1/swagger.json", "InventorLibraryManager WebAPI v1"));

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin()); // allow any origin

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>("/hubs/dashboard");
            });
        }
    }
}