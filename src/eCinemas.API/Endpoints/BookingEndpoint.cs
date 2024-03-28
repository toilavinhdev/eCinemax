using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Application.Commands;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class BookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/booking")
            .WithTags(nameof(Booking))
            .RequireAuthorization();

        group.MapPost("/checkout", (
                CreateBookingCommand command, 
                IMediator mediator) 
            => mediator.Send(command));
    }
}