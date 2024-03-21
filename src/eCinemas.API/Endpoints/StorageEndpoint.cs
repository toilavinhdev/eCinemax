using eCinemas.API.Application.Commands;
using eCinemas.API.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class StorageEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/storage").WithTags("Storage");

        group.MapPost("/upload", (
            IFormFile file,
            string? bucket,
            IMediator mediator) => mediator.Send(new UploadFileCommand(file, bucket)));
    }
}