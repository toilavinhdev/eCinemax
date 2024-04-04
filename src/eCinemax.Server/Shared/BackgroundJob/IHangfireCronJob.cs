using Hangfire;

namespace eCinemax.Server.Shared.BackgroundJob;

[AutomaticRetry(Attempts = 0)]
public interface IHangfireCronJob
{
    Task<string> Run();
}