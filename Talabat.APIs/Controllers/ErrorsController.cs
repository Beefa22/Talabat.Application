using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
	//For Handling NotFound endpoint response
	[Route("errors/{code}")]
	[ApiController]
	[ApiExplorerSettings(IgnoreApi =true)]
	public class ErrorsController : ControllerBase
	{
		public ActionResult Error(int code)
		{
			return NotFound(new ApiResponse(code));
		}
	}
}
