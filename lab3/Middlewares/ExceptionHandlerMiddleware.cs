using System.Net;
using System.Text;
using BLL.Exceptions;
using DAL.Exceptions;

namespace lab3.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next) => _next = next;
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (AlreadyAssignedException e)
        {   
            await SetMessageAndStatus(httpContext.Response, e.Message);
        }
        catch (ExperienceException e)
        {
            await SetMessageAndStatus(httpContext.Response, e.Message);
        }
        catch (ToManyAircraftsException e)
        {
            await SetMessageAndStatus(httpContext.Response, e.Message);
        }
        catch (EntityNotFoundException e)
        {
            await SetMessageAndStatus(httpContext.Response, e.Message);
        }
        catch (NotAssignedException e)
        {
            await SetMessageAndStatus(httpContext.Response, e.Message);
        }  
        catch (Exception e)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    private async Task SetMessageAndStatus(HttpResponse response, string message)
    {
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.ContentType = "text/plain";
        await response.Body.WriteAsync(ConvertToBytes(message));
    }
    private byte[] ConvertToBytes(string message)
    {
        return Encoding.UTF8.GetBytes(message);
    }
}
