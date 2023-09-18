using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Interfaces;
using Talabat.Repository;

namespace Talabat.APIs.Extentions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
			//webApplicationBuilder.Services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			//webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														 .SelectMany(p => p.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToArray();
					var ValidationErrorResponse = new ApiValidationErrorResponse()
					{
						Errors =errors
					};
					return new BadRequestObjectResult(ValidationErrorResponse);
				};

			});
			return services;
		}
	}
}
