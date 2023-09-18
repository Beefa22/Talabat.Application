using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class IdentityServicesExtension
	{
		public static IServiceCollection AddIdentityServices( this IServiceCollection services,IConfiguration configuration)
		{
			services.AddScoped<ITokenService, TokenService>();
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{
				//options.Password.RequireNonAlphanumeric = false;
				//options.Password.RequireDigit = false;
				//options.Password.RequiredLength= 4;
			})
			.AddEntityFrameworkStores<AppIdentityDbContext>();

			services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options=>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],

						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],

						ValidateLifetime = true,

						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))



					};
				});
			return services;
		}
	}
}
