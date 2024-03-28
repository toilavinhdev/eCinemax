using eCinemas.API.Aggregates.PaymentAggregate;
using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Endpoints;

public class PaymentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/payment")
            .WithTags(nameof(Payment))
            .RequireAuthorization();
    }
}