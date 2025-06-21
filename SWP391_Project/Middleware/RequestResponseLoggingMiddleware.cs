using System.Text;
using Microsoft.IO;

namespace SWP391_Project.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _streamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _streamManager = new RecyclableMemoryStreamManager();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            var requestBody = await ReadRequestBodyAsync(context.Request);
            _logger.LogInformation(
                "Request: {Method} {Path} {QueryString} {RequestBody}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                requestBody);

            // Log response
            var originalBodyStream = context.Response.Body;
            using var responseBody = _streamManager.GetStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                responseBody.Seek(0, SeekOrigin.Begin);
                var response = await new StreamReader(responseBody).ReadToEndAsync();
                _logger.LogInformation(
                    "Response: {StatusCode} {ResponseBody}",
                    context.Response.StatusCode,
                    response);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }
    }
} 