using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MeterReader.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeterReader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize]  - For this example no authorisation is being applied.  However, production code we would need all external endpoints
    // to be secured, possibly behind an API Gateway.  If this is not used then it would be necessary to support an appropriate
    // authentication / authorisation mechanism.  Something along the lines of the OAuth2 Authorized Code Flow might be appropriate.
    public class MeterReaderController : ControllerBase
    {
        // In general we need to include logging at major decisions points and always when things go wrong.
        // Logging shouldn't be confused with instrumentation.  An appropriate instrumentation approach should
        // be determined, for exampe AWS X-Ray technology.
        private readonly ILogger<MeterReaderController> _logger;

        private readonly IMeterReadingService _meterReadingService;

        public MeterReaderController(
            ILogger<MeterReaderController> logger,
            IMeterReadingService meterReadingService)
        {
            _logger = logger;
            _meterReadingService = meterReadingService;
        }

        // Do not try to post anything bigger than 30Mb - if this is expected then we need to consider
        // 1. Is a HttpPost the best way to upload the data?
        // 2. Can we get away with just increasing the limit?
        // 3. Should we stream the data?

        [HttpPost]
        [Route("/meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings()
        {
            try
            {
                // We may include additional logging steps here, or it may be more appropriate to add steps
                // that are common to all methods into the ASP .Net Core request pipeline as middleware.
                _logger.LogInformation("MeterReaderController:UploadMeterReadings");

                string content = await ReadBody();

                (int successfulMeterReadings, int failedReadings) = await _meterReadingService.ProcessReadings(content);

                return Ok(new
                {
                    successfulMeterReadings,
                    failedReadings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MeterReaderController:UploadMeterReadings: Unexpected exception");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        private async Task<string> ReadBody()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        [HttpGet]
        [Route("/duplicateMeterReadings")]
        public async Task<IActionResult> GetDuplicateMeterReadings()
        {
            try
            {
                _logger.LogInformation("MeterReaderController:GetDuplicateMeterReadings");

                var duplicates = await _meterReadingService.GetDuplicateMeterReadings();

                return Ok(duplicates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MeterReaderController:GetDuplicateCsvLines: Unexpected exception");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("/failedCsvLines")]
        public async Task<IActionResult> GetFailedCsvLines()
        {
            try
            {
                _logger.LogInformation("MeterReaderController:GetFailedCsvLines");

                var failedLines = await _meterReadingService.GetFailedCsvLines();

                return Ok(failedLines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MeterReaderController:GetFailedCsvLines: Unexpected exception");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("/meterReadings")]
        public async Task<IActionResult> GetMeterReadings()
        {
            try
            {
                _logger.LogInformation("MeterReaderController:GetMeterReadings");

                var readings = await _meterReadingService.GetMeterReadings();

                return Ok(readings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MeterReaderController:GetMeterReadings: Unexpected exception");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
