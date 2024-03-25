using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class ShowTimeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/show-time").WithTags(nameof(ShowTime));

        group.MapPost("create", (CreateShowTimeCommand command, IMediator mediator)
            => mediator.Send(command));
    }
}