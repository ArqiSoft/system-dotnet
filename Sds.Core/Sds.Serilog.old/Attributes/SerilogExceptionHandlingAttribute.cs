using Serilog;
using System.Web.Http.Filters;

namespace Sds.Serilog
{
    public class SerilogExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Log.Logger.Error(context.Exception, "Critical Exception");
        }
    }
}
