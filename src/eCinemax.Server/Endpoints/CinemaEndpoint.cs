using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Application.Commands.CinemaCommands;
using eCinemax.Server.Application.Queries.CinemaQueries;
using MediatR;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class CinemaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cinema").WithTags(nameof(Cinema));

        group.MapPost("/list", (IMediator mediator) 
            => mediator.Send(new ListCinemaQuery()));

        group.MapPost("/create", (CreateCinemaCommand command, IMediator mediator) 
            => mediator.Send(command))
            .RequireAuthorization();
    }
} 