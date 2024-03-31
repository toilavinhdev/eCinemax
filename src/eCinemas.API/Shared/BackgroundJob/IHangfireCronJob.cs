using Hangfire;

namespace eCinemas.API.Shared.BackgroundJob;

[AutomaticRetry(Attempts = 0)]
public interface IHangfireCronJob
{
    Task<string> Run();
}