using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Wing.Dashboard.Result;

namespace Wing.Dashboard.Filters
{
    public class ApiExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                context.Result = new ObjectResult(new ApiResult() { Code = ResultType.Exception, Msg = context.Exception.Message });
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
