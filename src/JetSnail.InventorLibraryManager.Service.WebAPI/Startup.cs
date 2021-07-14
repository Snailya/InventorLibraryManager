using System;
using System.IO;
using JetSnail.InventorLibraryManager.Data;
using JetSnail.InventorLibraryManager.Service.Services;
using JetSnail.InventorLibraryManager.UseCase.DataStores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JetSnail.InventorLibraryManager.Service.WebAPI
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
			// #region snippet_MigrationsAssembly
			// services.AddDbContext<ApplicationDbContext>(
			//     options =>
			//         options.UseSqlServer(
			//             Configuration.GetConnectionString("DefaultConnection"),
			//             x => x.MigrationsAssembly("JetSnail.InventorLibraryManager.Data")));
			services.AddDbContext<ContentCenterContext>(
				options =>
					options.UseSqlServer(
						Configuration.GetConnectionString("DefaultConnection"),
						x => { x.MigrationsAssembly("JetSnail.InventorLibraryManager.Data"); }));
			// #endregion

			services.AddRouting(options => options.LowercaseUrls = true);

			services.AddCors();

			services.AddControllers();
			// services.AddApiVersioning(config =>
			// {
			//     config.DefaultApiVersion = new ApiVersion(1, 0);
			//     config.AssumeDefaultVersionWhenUnspecified = true;
			// });
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1",
					new OpenApiInfo {Title = "JetSnail.InventorLibraryManager.Service.WebAPI", Version = "v1"});
				var basePath =
					Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
				var xmlPath = Path.Combine(basePath!, "JetSnail.InventorLibraryManager.Service.WebAPI.xml");
				c.IncludeXmlComments(xmlPath);
			});

			services.AddScoped<ILibraryRepository, LibraryRepository>();
			services.AddScoped<IFamilyRepository, FamilyRepository>();
			services.AddScoped<IGroupRepository, GroupRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventorLibraryManager WebAPI v1"));


			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors(x => x.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") // allow any origin
				.AllowCredentials()); // allow credentials

			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}