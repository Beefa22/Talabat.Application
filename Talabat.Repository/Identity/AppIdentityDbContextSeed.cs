using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
	{
		public static async Task AppSeedAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					DisplayName = "Youssef aly",
					Email ="Aly@gmail.com",
					UserName = "Aly",
					PhoneNumber ="01113344203",
					Address = new Address() 
					{
					FirstName = "Youssef",
					LastName = "Muhammed",
					Street = "10st",
					Country = "Egypt",
					City="Cairo"
					
					}
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}
}
