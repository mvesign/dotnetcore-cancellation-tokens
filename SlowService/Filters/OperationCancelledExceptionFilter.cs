using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace SlowService.Filters
{
    /// <inheritdoc />
    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public OperationCancelledExceptionFilter(
            ILoggerFactory loggerFactory
        )
        {
            _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
        }

        /// <inheritdoc />
        public override void OnException(
            ExceptionContext context
        )
        {
            if (!(context.Exception is OperationCanceledException))
                return;

            // Remove this part to enable proper exception handling when a cancellable request is cancelled.
            // For now leave this part in, for demo purposes.
            if (!"/slow/cancel-exception".Equals(context.HttpContext.Request.Path))
            {
                _logger.LogInformation("OperationCanceledException thrown but not handled in this flow.");
                return;
            }

            _logger.LogInformation("Request was cancelled");
            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult(400);
        }
    }
}