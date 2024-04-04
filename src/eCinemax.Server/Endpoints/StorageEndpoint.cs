using eCinemax.Server.Application.Commands.FileCommands;
using eCinemax.Server.Shared.ValueObjects;
using MediatR;

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