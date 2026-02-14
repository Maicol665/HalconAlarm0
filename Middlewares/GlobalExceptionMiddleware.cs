using System.Net;
using System.Text.Json;

namespace HalconAlarm0.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no manejado en la aplicación");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            object response;

            switch (exception)
            {
                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new
                    {
                        statusCode = 400,
                        mensaje = "Solicitud inválida",
                        detalle = exception.Message
                    };
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new
                    {
                        statusCode = 400,
                        mensaje = "Operación no válida",
                        detalle = exception.Message
                    };
                    break;

                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = new
                    {
                        statusCode = 404,
                        mensaje = "Recurso no encontrado",
                        detalle = exception.Message
                    };
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = new
                    {
                        statusCode = 500,
                        mensaje = "Error interno del servidor",
                        detalle = "Ha ocurrido un error inesperado. Contacte al administrador."
                    };
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}