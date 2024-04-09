using eCinemax.Server.Aggregates.BookingAggregate;
using eCinemax.Server.Application.Commands.BookingCommands;
using eCinemax.Server.Application.Queries.BookingQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class BookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/booking").WithTags(nameof(Booking));
        
        group.MapPost("/get", (GetBookingQuery query, IMediator mediator) => mediator.Send(query))
            .RequireAuthorization();

        group.MapPost("/create", async (CreateBookingCommand command, IMediator mediator) =>
            {
                var bookingId = await mediator.Send(command);
                var bookingDetail = await mediator
                    .Send(new GetBookingQuery{ Id = bookingId });
                return bookingDetail;
            })
            .RequireAuthorization();

        group.MapPost("/checkout", (CreatePaymentCommand command, IMediator mediator) => mediator.Send(command))
            .RequireAuthorization();

        group.MapGet("/vnpay-return",
            ([AsParameters] ProcessVnPayReturnCommand command, 
                [FromServices] IMediator mediator) => mediator.Send(command));
        
        group.MapGet("/vnpay-ipn",
            ([AsParameters] ProcessVnPayIPNCommand command, 
                [FromServices] IMediator mediator) => mediator.Send(command));
    }
}