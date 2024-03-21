using eCinemas.API.Mediator;
using eCinemas.API.Services;
using eCinemas.API.ValueObjects;
using FluentValidation;

namespace eCinemas.API.Application.Commands;

public record UploadFileCommand(IFormFile File, string? Bucket) : IAPIRequest<string>;

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
        throw new NotImplementedException();
    }
}