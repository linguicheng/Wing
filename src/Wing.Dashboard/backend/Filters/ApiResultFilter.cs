using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Wing.Dashboard.Result;

namespace Wing.Dashboard.Filters
{
    public class ApiResultFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value is ApiResult result)
                {
                    context.Result = new ObjectResult(result);
                    return;
                }
                context.Result = new ObjectResult(new ApiResult<object> { Code = ResultType.Success, Data = objectResult.Value });
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new ApiResult());
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new ApiResult<string> { Data = (context.Result as ContentResult).Content });
            }
            else if (context.Result is Exception)
            {
                var result = context.Result as Exception;
                context.Result = new ObjectResult(new ApiResult() { Code = ResultType.Exception, Msg = result.Message });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
