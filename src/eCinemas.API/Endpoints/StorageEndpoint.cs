using eCinemas.API.Application.Commands.FileCommands;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

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