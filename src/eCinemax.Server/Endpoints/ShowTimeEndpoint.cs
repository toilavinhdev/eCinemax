using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Application.Commands.ShowTimeCommands;
using eCinemax.Server.Application.Queries.ShowTimeQueries;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class ShowTimeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/showtime").WithTags(nameof(ShowTime));

        group.MapPost("/get", (GetShowTimeQuery query, IMediator mediator) 
            => mediator.Send(query));
        
        group.MapPost("/list", (ListShowTimeQuery query, IMediator mediator) 
            => mediator.Send(query));

        group.MapPost("/create", (CreateShowTimeCommand command, IMediator mediator)
            => mediator.Send(command))
            .RequireAuthorization();

        group.MapPost("/all", (ListAllShowTimeQuery query, IMediator mediator) => mediator.Send(query))
            .RequireAuthorization();
    }
}