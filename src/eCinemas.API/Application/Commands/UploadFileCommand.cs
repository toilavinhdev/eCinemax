using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;

namespace eCinemas.API.Application.Commands;

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

public class UploadFileCommandHandler(IStorageService storageService) : IAPIRequestHandler<UploadFileCommand, string>
{
    public async Task<APIResponse<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(request.File.FileName)}";
        var url = await storageService.SaveAsync(request.File, fileName, request.Bucket, cancellationToken);
        return APIResponse<string>.IsSuccess(url, "Upload thành công");
    }
}