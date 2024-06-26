﻿using eCinemax.Server.Application.Commands.ShowTimeCommands;
using Todo.NET.Hangfire;

namespace eCinemax.Server.Schedule;

[Hangfire("*/1 * * * *", "Execute at every 1 minutes")]
public class ShowTimeStatusTrackingService(ILogger<ShowTimeStatusTrackingService> logger, ISender sender) : IHangfireCronJob
{
    public async Task<string> Run()
    {
        logger.LogInformation("Starting job: {Job}", nameof(ShowTimeStatusTrackingService));
        await sender.Send(new UpdateShowTimeStatusCommand());
        logger.LogInformation("Finished job: {Job}", nameof(ShowTimeStatusTrackingService));
        return await Task.FromResult($"Finished job {nameof(ShowTimeStatusTrackingService)}");
    }
}