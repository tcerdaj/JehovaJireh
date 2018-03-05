using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace JehovaJireh.Web.Services.ErrorHandler
{
	public class OopsExceptionHandler : ExceptionHandler
	{
		public override void Handle(ExceptionHandlerContext context)
		{
			context.Result = new ErrorResult(context.ExceptionContext.Exception, context.ExceptionContext.Request);
		}
	}
}