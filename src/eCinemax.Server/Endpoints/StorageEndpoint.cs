﻿using eCinemax.Server.Application.Commands.FileCommands;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class StorageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/storage").WithTags("Storage");

        group.MapPost("/upload", (IFormFile file, string? bucket, IMediator mediator)
            => mediator.Send(new UploadFileCommand { File = file, Bucket = bucket }))
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}