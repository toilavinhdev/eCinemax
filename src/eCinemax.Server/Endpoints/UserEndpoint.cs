using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Commands.UserCommands;
using eCinemax.Server.Application.Queries.UserQueries;
using MediatR;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

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