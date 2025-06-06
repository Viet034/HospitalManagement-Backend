using Quartz;

namespace SWP391_SE1914_ManageHospital.Ultility;

public class MyCronJob : IJob
{
    private readonly ILogger<MyCronJob> _logger;

    public MyCronJob(ILogger<MyCronJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("MyCronJob is running at: {Time}", DateTime.Now);
        return Task.CompletedTask;
    }
}
