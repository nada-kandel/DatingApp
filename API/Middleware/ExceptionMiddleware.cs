using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next 
, ILogger<ExceptionMiddleware>logger,IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);   // 1. الباس request للي بعده
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);  // 2. سجل الخطأ

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // 3. بنبني object فيه تفاصيل الخطأ
            var response = env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error");

            // 4. تحويله JSON قبل الإرسال
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);

            // 5. نرجّع JSON للعميل
            await context.Response.WriteAsync(json);
        }
    }

}
