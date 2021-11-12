
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.API.Controllers;
using ShoppingAPI.Database.Data.Services.Exceptions;

namespace ShoppingAPI.API.Middlewares
{
  public class ExceptionHandling : IMiddleware
  {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      try
      {
        await next(context);
      }
      catch (Exception ex)
      {
        var exceptionType = ex.GetType().ToString();
        var exception = ex.InnerException ?? ex;

        ObjectResult result = null;

        switch (exception)
        {
          case NotEnoughProductsInStock:
            {
              var e = exception as NotEnoughProductsInStock;
              result = new BadRequestObjectResult(new APIResponse()
              {
                Message = e.Message,
                Data = e.productQuantities,
              });
              break;
            }

          case DoesNotExist:
            {
              var e = exception as DoesNotExist;
              result = new BadRequestObjectResult(new APIResponse()
              {
                Message = e.Message,
                Data = e.Ids,
              });
              break;
            }

          case EmptyCart:
            {
              var e = exception as EmptyCart;
              result = new BadRequestObjectResult(new APIResponse()
              {
                Message = e.Message,
              });
              break;
            }

          default:
            throw;
        }

        if (result is not null)
        {
          context.Response.StatusCode = result.StatusCode ?? context.Response.StatusCode;
          await context.Response.WriteAsJsonAsync(result.Value);
        }
      }
    }
  }
}
