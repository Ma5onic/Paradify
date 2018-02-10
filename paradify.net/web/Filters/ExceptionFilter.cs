using log4net;
using System;
using System.Web.Mvc;

namespace web.Filters
{
    public class CustomHandleError : HandleErrorAttribute
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(CustomHandleError));

       

        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            log.Error("", filterContext.Exception);
           
        }


    }
}