using eCinemas.API.Application.Commands;
using eCinemas.API.Application.Queries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class UserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user").WithTags("User"); 
        
        group.MapGet("/me", (ISender sender) => sender.Send(new GetMeQuery()))
             .RequireAuthorization();

        group.MapPost("/sign-in", (SignInCommand command, ISender sender) => sender.Send(command));
        
        group.MapPost("/sign-up", (SignUpCommand command, ISender sender) => sender.Send(command));
    }
}