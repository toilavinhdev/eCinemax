using eCinemax.Server.Persistence;
using eCinemax.Server.Services;
using MongoDB.Driver;
using FileAggregate_File = eCinemax.Server.Aggregates.FileAggregate.File;

namespace eCinemax.Server.Application.Commands.FileCommands;

public class UploadFileCommand : IAPIRequest<string>
{
    public IFormFile File { get; set; } = default!;
    
    public string? Bucket { get; set; }
}

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File).NotEmpty();
    }
}

public class UploadFileCommandHandler(IStorageService storageService, IMongoService mongoService) : IAPIRequestHandler<UploadFileCommand, string>
{
    private readonly IMongoCollection<FileAggregate_File> _fileCollection = mongoService.Collection<FileAggregate_File>();
    
    public async Task<APIResponse<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(request.File.FileName)}";
        var path = await storageService.SaveAsync(request.File, fileName, request.Bucket, cancellationToken);

        var document = new FileAggregate_File
        {
            SourceName = request.File.FileName,
            FileName = fileName,
            Path = path,
            Size = request.File.Length,
            ContentType = request.File.ContentType
        };
        document.MarkCreated(mongoService.UserClaims().Id);
        await _fileCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return APIResponse<string>.IsSuccess(path, "Upload thành công");
    }
}