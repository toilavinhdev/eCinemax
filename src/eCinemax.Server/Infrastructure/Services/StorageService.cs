using eCinemax.Server.Shared.ValueObjects;

namespace eCinemax.Server.Infrastructure.Services;

public interface IStorageService : IBaseService
{
    Task<string> SaveAsync(IFormFile file, string fileName, string? bucket, CancellationToken cancellationToken);
}

public class StorageService(IHttpContextAccessor httpContextAccessor, AppSettings appSettings) 
    : BaseService(httpContextAccessor), IStorageService
{
    public async Task<string> SaveAsync(IFormFile file, 
                                        string fileName, 
                                        string? bucket = null, 
                                        CancellationToken cancellationToken = new())
    {
        var root = appSettings.StaticFileConfig.Location;
        var bucketToFile = string.IsNullOrEmpty(bucket) 
            ? fileName 
            : InitialBucket(Path.Combine(bucket, fileName));
        var rootToFile = Path.Combine(root, bucketToFile);
 
        await using var stream = new FileStream(rootToFile, FileMode.Create); 
        await file.CopyToAsync(stream, cancellationToken);

        return GetUrl(bucketToFile);
    }

    private string GetUrl(string bucketToFile)
    {
        return $"{appSettings.Host}{appSettings.StaticFileConfig.External}/{bucketToFile}";
    }
    
    private string InitialBucket(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }
}