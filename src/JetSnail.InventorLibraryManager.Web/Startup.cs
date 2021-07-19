using System;
using FluentValidation;
using JetSnail.InventorLibraryManager.UseCase.UseCases;
using JetSnail.InventorLibraryManager.Web.Areas.Identity;
using JetSnail.InventorLibraryManager.Web.Data;
using JetSnail.InventorLibraryManager.Web.Validators;
using JetSnail.InventorLibraryManager.Web.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JetSnail.InventorLibraryManager.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlite(
					Configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services
				.AddScoped<AuthenticationStateProvider,
					RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddAntDesign();

			services.AddHttpClient("inventor",
				config => config.BaseAddress = new Uri(Configuration["HttpClientBaseAddress"]));

			services.AddTransient<IValidator<GroupLineItemViewModel>, GroupValidator>();

			services.AddScoped<IGetGroupsUseCase, GetGroupsUseCase>();
			services.AddScoped<IAddGroupUseCase, AddGroupUseCase>();
			services.AddScoped<IDeleteGroupUseCase, DeleteGroupUseCase>();

			services.AddScoped<IGetFamiliesUseCase, GetFamiliesUseCase>();
			services.AddScoped<IAddFamilyUseCase, AddFamilyUseCase>();
			services.AddScoped<IUpdateFamilyUseCase, UpdateFamilyUseCase>();
			services.AddScoped<IGetFamilyPartsUseCase, GetFamilyPartsUseCase>();
			services.AddScoped<IUpdateFamilyPartsUseCase, UpdateFamilyPartsUseCase>();
			services.AddScoped<IUpdateFamilyPartUseCase, UpdateFamilyPartUseCase>();

			services.AddScoped<IGetLibrariesUseCase, GetLibrariesUseCase>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}