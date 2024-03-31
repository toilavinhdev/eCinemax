using eCinemas.API.Application.Commands.ShowTimeCommands;
using eCinemas.API.Shared.BackgroundJob;
using MediatR;

namespace eCinemas.API.Services;

[Hangfire("*/1 * * * *", "Execute at every 1 minutes")]
public class UpdateShowTimeStatusService(ILogger<UpdateShowTimeStatusService> logger, ISender sender) : IHangfireCronJob
{
    public async Task<string> Run()
    {
        logger.LogInformation("Starting job: {Job}", nameof(UpdateShowTimeStatusService));
        await sender.Send(new UpdateShowTimeStatusCommand());
        logger.LogInformation("Finished job: {Job}", nameof(UpdateShowTimeStatusService));
        return await Task.FromResult($"Finished job {nameof(UpdateShowTimeStatusService)}");
    }
}