# Slow Service
Small web application created to demo the CancellationTokens in dotnetcore. 

### How to use this solution

###### 1. Open solution in Visual Studio
```PowerShell
devenv.eve SlowService.sln
```

###### 2. Build solutions
```
[F6] / [Ctrl]-[Shift]-[B]
```

###### 3. Run web application
```
[F5]
```

### Perform requests

The idea of this project is to perform one request twice. When the second request is performed, the first request is cancelled due to the usaged of CancellationTokens.

###### 1. Perform a slow GET request.

```
GET http://localhost:57675/slow HTTP/1.1
```

Will execute both requests without cancellation or exceptions:

```
// First request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "Get", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.Get (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.Get (SlowService) - Validation state: Valid

// Second request	  
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "Get", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.Get (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.Get (SlowService) - Validation state: Valid

// Response of the first request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:10.0133821
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.Get (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 10021.3383ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.Get (SlowService) in 10108.5738ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 10351.6277ms 200 text/plain; charset=utf-8

// Response of the second request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:10.0096070
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.Get (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 10012.1102ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.Get (SlowService) in 10044.4704ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 10052.4859ms 200 text/plain; charset=utf-8
```

###### 2. Perform a slow GET request with a cancellation token.

```
GET http://localhost:57675/slow/cancel HTTP/1.1
```

Will throw a cancellation exception, that is never caught or handled properly. This is visible in the console logging.

```
// First request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/cancel
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCancellationAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid

// Second request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/cancel
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCancellationAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid
	  
// Exception of first request being cancelled
info: SlowService.Filters.OperationCancelledExceptionFilter[0]
      OperationCanceledException thrown but not handled in this flow.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService) in 1166.8226ms
fail: Microsoft.AspNetCore.Server.Kestrel[13]
      Connection id "0HLJ7AC7PT5SM", Request id "0HLJ7AC7PT5SM:00000001": An unhandled exception was thrown by the application.
System.Threading.Tasks.TaskCanceledException: A task was canceled.
   at SlowService.Controllers.SlowController.GetWithCancellationAsync(CancellationToken cancellationToken) in C:\Projects\Github\mvesign\dotnetcore-cancellation-tokens\SlowService\Controllers\SlowController.cs:line 56
   at Microsoft.AspNetCore.Mvc.Internal.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at System.Threading.Tasks.ValueTask`1.get_Result()
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeActionMethodAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeNextActionFilterAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Rethrow(ActionExecutedContext context)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker.InvokeInnerFilterAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeNextExceptionFilterAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Rethrow(ExceptionContext context)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeNextResourceFilter()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Rethrow(ResourceExecutedContext context)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeFilterPipelineAsync()
   at Microsoft.AspNetCore.Mvc.Internal.ResourceInvoker.InvokeAsync()
   at Microsoft.AspNetCore.Builder.RouterMiddleware.Invoke(HttpContext httpContext)
   at Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware.Invoke(HttpContext context)
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpProtocol.ProcessRequests[TContext](IHttpApplication`1 application)
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 1480.6095ms 0

// Response of the second request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:10.0123677
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 10015.2431ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCancellationAsync (SlowService) in 10054.6413ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 10063.3276ms 200 text/plain; charset=utf-8
```

###### 3. Perform a slow GET request with a cancellation token that is checked.

```
GET http://localhost:57675/slow/check-cancel HTTP/1.1
```

No cancellation exception, but the cancelled state is checked. Based on this the processed is either stopped or continued.

```
// First request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/check-cancel
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCheckCancellationAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid

// Second request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/check-cancel
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCheckCancellationAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid

// Response of the first request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:02.0008525
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 2004.2162ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService) in 2110.9367ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 2350.6545ms 200 text/plain; charset=utf-8

// Response of the second request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:10.0040955
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 10007.3597ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCheckCancellationAsync (SlowService) in 10037.2252ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 10046.0514ms 200 text/plain; charset=utf-8
```

###### 4. Perform a slow GET request with a cancellation token that throws an OperationCancelledException exception.

```
GET http://localhost:57675/slow/cancel-exception HTTP/1.1
```

Will throw a cancellation exception, which is handled and converted to a HTTP 400 response.

```
// First request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/cancel-exception
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCancellationExceptionAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid

// Second request
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[1]
      Request starting HTTP/1.1 GET http://localhost:5000/slow/cancel-exception
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Route matched with {action = "GetWithCancellationExceptionAsync", controller = "Slow"}. Executing action SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService)
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[1]
      Executing action method SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService) with arguments (System.Threading.CancellationToken) - Validation state: Valid

// Exception of the first request, being handled.
info: SlowService.Filters.OperationCancelledExceptionFilter[0]
      Request was cancelled
info: Microsoft.AspNetCore.Mvc.StatusCodeResult[1]
      Executing HttpStatusCodeResult, setting HTTP status code 400
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService) in 1248.4695ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 1490.4235ms 0

// Response of the second request
info: SlowService.Controllers.SlowController[0]
      Finished work in 00:00:10.0019525
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action method SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService), returned result Microsoft.AspNetCore.Mvc.OkObjectResult in 10005.2535ms.
info: Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor[1]
      Executing ObjectResult, writing value of type 'System.String'.
info: Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker[2]
      Executed action SlowService.Controllers.SlowController.GetWithCancellationExceptionAsync (SlowService) in 10043.7032ms
info: Microsoft.AspNetCore.Hosting.Internal.WebHost[2]
      Request finished in 10060.3922ms 200 text/plain; charset=utf-8
```