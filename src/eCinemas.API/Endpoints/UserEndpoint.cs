using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Application.Commands.UserCommands;
using eCinemas.API.Application.Queries.UserQueries;
using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Endpoints;

public class UserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user").WithTags(nameof(User)); 
        
        group.MapPost("/me", (ISender sender) => sender.Send(new GetMeQuery()))
             .RequireAuthorization();

        group.MapPost("/sign-in", (SignInCommand command, ISender sender) => sender.Send(command));
        
        group.MapPost("/sign-up", (SignUpCommand command, ISender sender) => sender.Send(command));

        group.MapPut("/update-profile", (UpdateProfileCommand command, ISender sender) => sender.Send(command))
             .RequireAuthorization();
        
        group.MapPut("/update-password", (UpdatePasswordCommand command, ISender sender) => sender.Send(command))
             .RequireAuthorization();

        // group.MapPost("/forgot-password", (ForgotPasswordCommand command, ISender sender) => sender.Send(command));
    }
}