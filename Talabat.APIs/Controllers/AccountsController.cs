using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{

    public class AccountsController : BaseController
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;

		public AccountsController(ITokenService tokenService,
			UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager,
			IMapper mapper)
		{
			_tokenService = tokenService;
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
		}

		[HttpPost("Login")]//Post : api/account/Login
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new ApiResponse(401));
			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token =await _tokenService.CreateTokenAsync(user, _userManager)
			}) ;

		}

		[HttpPost("register")]//Post: /api/Account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{

			if (CheckEmailExistance(model.Email).Result.Value)//Note.Result to make it Synchrounous To block the rest of the code if the email exists.
				return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is already Exists!" } });

			var user = new AppUser()
			{
				DisplayName = model.DisplayName,
				Email = model.Email,
				UserName = model.Email.Split('@')[0],
				PhoneNumber = model.PhoneNumber,
			};
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token =await _tokenService.CreateTokenAsync(user,_userManager)
			});
		}

		[HttpGet("address")]
		[Authorize]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{
			//var email = User.FindFirstValue(ClaimTypes.Email);
			//var user = await _userManager.FindByEmailAsync(email);
			var user = await _userManager.FindUserWithAddressAsync(User);

			var address = _mapper.Map<Address, AddressDto>(user.Address);

			return Ok(address);
		}

		[HttpPut("address")]//Put : /api/Accounts/address
		[Authorize]
		public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
		{
			var mappedAddress = _mapper.Map<AddressDto, Address>(updatedAddress);
			var user = await _userManager.FindUserWithAddressAsync(User);

			mappedAddress.Id = user.Address.Id;//to Edit the same record not delete the old record and create new one.

			user.Address = mappedAddress;

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded) return BadRequest(new ApiResponse(400));
			return (updatedAddress);
		}

		[HttpGet]
		public async Task<ActionResult<bool>> CheckEmailExistance(string email) {
			return await _userManager.FindByEmailAsync(email) is not null;
				}

	}
}
