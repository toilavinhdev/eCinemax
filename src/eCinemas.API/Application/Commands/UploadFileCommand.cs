﻿using eCinemas.API.Aggregates.FileAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

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

public class UploadFileCommandHandler(IStorageService storageService, IMongoService mongoService) : IAPIRequestHandler<UploadFileCommand, string>
{
    private readonly IMongoCollection<ApplicationFile> _fileCollection = mongoService.Collection<ApplicationFile>();
    
    public async Task<APIResponse<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(request.File.FileName)}";
        var path = await storageService.SaveAsync(request.File, fileName, request.Bucket, cancellationToken);

        var document = new ApplicationFile
        {
            SourceName = request.File.FileName,
            FileName = fileName,
            Path = path,
            Size = request.File.Length,
            ContentType = request.File.ContentType
        };
        document.MarkCreated(mongoService.GetUserClaimValue()?.Id);
        await _fileCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return APIResponse<string>.IsSuccess(path, "Upload thành công");
    }
}