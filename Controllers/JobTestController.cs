using Hangfire;
using HangFire2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HangFire2.Controllers
{
[Route("api/[controller]")]
[ApiController]

public class JobTestController : ControllerBase
{
    private readonly IjobTestService _jobTestService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    public JobTestController(IjobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
    {
        _jobTestService = jobTestService;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
    }
    [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            return Ok();
        }
    [HttpGet("/DelayedJob")]
    public ActionResult CreateDelayedJob()
    {
        _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));
        return Ok();
    }
    [HttpGet("/ReccuringJob")]
    public ActionResult CreateReccuringJob()
    {
       
        _recurringJobManager.AddOrUpdate("Teste", () => _jobTestService.ReccuringJob(), Cron.Monthly(5));
        return Ok();
    }
    [HttpGet("/ContinuationJob")]
    public ActionResult CreateContinuationJob()
    {
        var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
        _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());
                
        return Ok();
    }

    }
}