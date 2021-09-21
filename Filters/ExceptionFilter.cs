

using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShoppingAPI.Controllers;
using ShoppingAPI.Data.Repositories.Exceptions;
using ShoppingAPI.Data.Repositories.Records;

namespace ShoppingAPI.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
      public override void OnException(ExceptionContext context)
      {
        base.OnException(context);

        var exceptionType = context.Exception.GetType().ToString();
        var exception = context.Exception.InnerException ?? context.Exception;

        ObjectResult result = null;
        
        switch (exception)
        {
            case NotEnoughProductsInStock:
            {
              var e = exception as NotEnoughProductsInStock;
              result = new BadRequestObjectResult(new APIResponse() { 
                Message = e.Message,
                Data = e.productQuantities,
              });
              break;
            }

            case DoesNotExist:
            {
              var e = exception as DoesNotExist;
              result = new BadRequestObjectResult(new APIResponse() { 
                Message = e.Message,
                Data = e.Ids,
              });
              break;
            }

            case EmptyCart:
            {
              var e = exception as EmptyCart;
              result = new BadRequestObjectResult(new APIResponse() { 
                Message = e.Message,
              });
              break;
            }

            default:
              break;
        }

        if (result is not null)
        {
          context.Result = result;
        }
      }
    }
}
