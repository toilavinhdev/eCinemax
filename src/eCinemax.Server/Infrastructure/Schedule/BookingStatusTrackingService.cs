using eCinemax.Server.Application.Commands.BookingCommands;
using eCinemax.Server.Shared.BackgroundJob;
using MediatR;

namespace eCinemax.Server.Infrastructure.Schedule;

[Hangfire("*/1 * * * *", "Execute at every 1 minutes")]
public class BookingStatusTrackingService(ISender sender, ILogger<BookingStatusTrackingService> logger) : IHangfireCronJob
{
    public async Task<string> Run()
    {
        logger.LogInformation("Starting job: {Job}", nameof(BookingStatusTrackingService));
        await sender.Send(new UpdateBookingExpiredCommand());
        logger.LogInformation("Finished job: {Job}", nameof(BookingStatusTrackingService));
        return await Task.FromResult($"Finished job {nameof(BookingStatusTrackingService)}");
    }
}