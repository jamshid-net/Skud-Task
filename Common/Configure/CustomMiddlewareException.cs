using System.Net;
using System.Text.Json;
using Common.CustomExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Common.Configure;

public static class CustomMiddlewareException
{
    public static IApplicationBuilder CustomExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ModelIsNullException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.UnprocessableContent); //422
        }
        catch (ValidationException ex)
        { 
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.UnprocessableContent); //422
        }
        catch(AlreadyExistException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Conflict); //409
        }
        catch(ConflictException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Conflict); //409
        }
        catch (AccessDeniedException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Forbidden); //403
        }
        catch (FileNotFoundException ex) 
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound); //404
        }
        catch (NotFoundException ex) 
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.NotFound); //404
        }
        catch (ErrorFromClientException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest); //400 
        }
        catch(RefreshTokenExpiredException ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.RequestTimeout); //408
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError); //500
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode httpStatusCode)
    {
        Log.Error($":Type of exception : {exception.GetType().Name}; Message: {exception.Message}");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)httpStatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = context?.Response?.StatusCode ?? 400,
            Message = exception?.Message ?? "Bad request!"
        }));

    }

}

