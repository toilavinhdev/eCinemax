using eCinemas.API.Application.Commands;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class UserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user").WithTags("User");

        group.MapPost("sign-in", (SignInCommand command, ISender sender) => sender.Send(command));
        
        group.MapPost("sign-up", (SignUpCommand command, ISender sender) => sender.Send(command));
    }
}