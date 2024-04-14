using eCinemax.Server.Aggregates.NotificationAggregate;
using eCinemax.Server.Application.Commands.NotificationCommands;
using eCinemax.Server.Application.Queries.NotificationQuery;
using MediatR;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class NotificationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notification").WithTags(nameof(Notification));

        group.MapPost("/list", (ListNotificationQuery query, IMediator mediator) => mediator.Send(query)).RequireAuthorization();

        group.MapPost("/create", (CreateNotificationCommand command, IMediator mediator) => mediator.Send(command));
    }
}