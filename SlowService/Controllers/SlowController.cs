using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlowService.Controllers
{
    /// <inheritdoc />
    [Route("slow")]
    public class SlowController : Controller
    {
        private readonly ILogger<SlowController> _logger;

        /// <summary>
        /// Create an instance of the SlowCancel controller.
        /// </summary>
        /// <param name="logger">Details of the logger.</param>
        public SlowController(
            ILogger<SlowController> logger
        )
        {
            _logger = logger;
        }

        /// <summary>
        /// Perform a slow GET request.
        /// </summary>
        /// <returns>Message with the execution time.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var startOfProcess = DateTime.UtcNow;

            await Task.Delay(10_000);

            var message = $"Finished work in {DateTime.UtcNow - startOfProcess}";

            _logger.LogInformation(message);

            return Ok(message);
        }

        /// <summary>
        /// Perform a slow GET request with a cancellation token.
        /// </summary>
        /// <param name="cancellationToken">Details of the cancellation token.</param>
        /// <returns>Message with the execution time.</returns>
        [HttpGet("cancel")]
        public async Task<IActionResult> GetWithCancellationAsync(
            CancellationToken cancellationToken
        )
        {
            var startOfProcess = DateTime.UtcNow;

            await Task.Delay(10_000, cancellationToken);

            var message = $"Finished work in {DateTime.UtcNow - startOfProcess}";

            _logger.LogInformation(message);

            return Ok(message);
        }

        /// <summary>
        /// Perform a slow GET request with a cancellation token that throws an OperationCancelledException exception.
        /// </summary>
        /// <param name="cancellationToken">Details of the cancellation token.</param>
        /// <returns>Message with the execution time.</returns>
        [HttpGet("cancel-exception")]
        public async Task<IActionResult> GetWithCancellationExceptionAsync(
            CancellationToken cancellationToken
        )
        {
            var startOfProcess = DateTime.UtcNow;

            await Task.Delay(10_000, cancellationToken);

            var message = $"Finished work in {DateTime.UtcNow - startOfProcess}";

            _logger.LogInformation(message);

            return Ok(message);
        }

        /// <summary>
        /// Perform a slow GET request with a cancellation token that is checked.
        /// </summary>
        /// <param name="cancellationToken">Details of the cancellation token.</param>
        /// <returns>Message with the execution time.</returns>
        [HttpGet("check-cancel")]
        public IActionResult GetWithCheckCancellationAsync(
            CancellationToken cancellationToken
        )
        {
            var startOfProcess = DateTime.UtcNow;

            for (var count = 0; count < 10; count++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Thread.Sleep(1000);
            }

            var message = $"Finished work in {DateTime.UtcNow - startOfProcess}";

            _logger.LogInformation(message);

            return Ok(message);
        }
    }
}