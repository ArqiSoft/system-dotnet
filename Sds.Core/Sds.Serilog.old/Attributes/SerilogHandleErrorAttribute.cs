using Serilog;
using System.Web.Mvc;

namespace Sds.Serilog
{
    public class SerilogHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Log.Logger.Error(filterContext.Exception, "Critical Exception");
        }
    }
}
