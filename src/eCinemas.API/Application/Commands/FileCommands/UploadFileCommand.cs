﻿using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;
using File = eCinemas.API.Aggregates.FileAggregate.File;

namespace eCinemas.API.Application.Commands.FileCommands;

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
    private readonly IMongoCollection<File> _fileCollection = mongoService.Collection<File>();
    
    public async Task<APIResponse<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(request.File.FileName)}";
        var path = await storageService.SaveAsync(request.File, fileName, request.Bucket, cancellationToken);

        var document = new File
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