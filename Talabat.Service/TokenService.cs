using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
		{
			
			//Private Claims
			var authClaims = new List<Claim>()
			{
			new Claim(ClaimTypes.GivenName,user.DisplayName),
			new Claim(ClaimTypes.Email,user.Email)
			};
			var userRoles = await userManager.GetRolesAsync(user);//To add roles claims in Private Claims
			foreach (var role in userRoles)
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			
			//Key
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
			
			//Create object from Token
			var token = new JwtSecurityToken(
				//RegisterClaims
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				//Private Claims
				claims: authClaims,
				//Key and Algorithm of Header
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
				);

			//To Create Token
		return	new JwtSecurityTokenHandler().WriteToken(token);




		}
	}
}
