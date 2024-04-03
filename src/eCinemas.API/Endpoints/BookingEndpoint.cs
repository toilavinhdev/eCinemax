using eCinemas.API.Aggregates.BookingAggregate;
using eCinemas.API.Application.Commands.BookingCommands;
using eCinemas.API.Shared.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace eCinemas.API.Endpoints;

public class BookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/booking").WithTags(nameof(Booking));

        group.MapPost("/create-booking", (CreateBookingCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();

        group.MapPost("/create-payment", (CreatePaymentCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();

        group.MapGet("/vnpay-return",
            ([AsParameters] ProcessVnPayReturnCommand command, 
                [FromServices] IMediator mediator) => mediator.Send(command));
        
        group.MapGet("/vnpay-ipn",
            ([AsParameters] ProcessVnPayIPNCommand command, 
                [FromServices] IMediator mediator) => mediator.Send(command));
    }
}