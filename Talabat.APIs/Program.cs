using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;
using Talabat.Core.IRepositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddSwaggerServices();

			builder.Services.AddDbContext<StoreContext>(options=>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")); 
			});

			builder.Services.AddIdentityServices(builder.Configuration);
			
			builder.Services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));

			builder.Services.AddApplicationServices();
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", options =>
				options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]));
			});


			#endregion

			var app = builder.Build();

			using var scope = app.Services.CreateScope(); //Create manual Scope [Explicitly]
			//Note i used the keyword"using" to despose database connection [close the connection fo DB]

			var services = scope.ServiceProvider;//service provider is object contain all services in scope

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				var dbContext = services.GetRequiredService<StoreContext>();//Ask Clr for creating object from DbContext Explicitly 
				await dbContext.Database.MigrateAsync();//Update-DataBase

				await StoreContextSeed.SeedAsync(dbContext);

				var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
				await IdentityDbContext.Database.MigrateAsync();//Update-DataBase

				var userManager = services.GetRequiredService<UserManager<AppUser>>();
				await AppIdentityDbContextSeed.AppSeedAsync(userManager);
 			}
			catch (Exception ex)
			{

				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error occured during apply the migration!");
			}

			#region Configure App [Kesteral] MiddleWares 
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddleWare();
			}

			app.UseStatusCodePagesWithRedirects("/error/{0}");//For Handling NotFound endpoint response

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseCors("MyPolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			
			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}