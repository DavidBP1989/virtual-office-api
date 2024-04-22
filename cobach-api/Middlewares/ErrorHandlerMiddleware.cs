using cobach_api.Exceptions;
using cobach_api.Wrappers;
using System.Net;
using System.Text.Json;

namespace cobach_api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        readonly RequestDelegate _handler;
        public ErrorHandlerMiddleware(RequestDelegate handler)
        {
            _handler = handler;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _handler(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new ApiResponse<string> { Succeeded = false, Message = ex?.Message };
                response.StatusCode = ex switch
                {
                    ApiException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
