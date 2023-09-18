namespace Talabat.APIs.Errors
{
	public class ApiResponse
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public ApiResponse(int statusCode,string? message =null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400=>"You made a Bad Request!",
				401=>"You are not Authorized",
				404=>"Resources are not found",
				500=> " Uh-oh! Our servers are having a bad day. We're on it, though! Please try again later"
			};
		}
	}
}
