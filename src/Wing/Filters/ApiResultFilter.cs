using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wing.Result;

namespace Wing.Filters
{
    public class ApiResultFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;

                if (objectResult.Value is ApiResult)
                {
                    context.Result = new ObjectResult(objectResult.Value);
                    return;
                }

                if (objectResult.Value is ApiResult<object>)
                {
                    context.Result = new ObjectResult(objectResult.Value);
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
            if (!context.ModelState.IsValid)
            {
                string msg = string.Empty;
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        msg += error.ErrorMessage + ",";
                    }
                }

                msg = msg.TrimEnd(',');
                context.Result = new ObjectResult(new ApiResult() { Code = ResultType.Exception, Msg = msg });
            }
        }
    }
}
