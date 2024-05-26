using eCinemax.Server.Services;
using Todo.NET.Extensions;

namespace eCinemax.Server.Endpoints;

public class TestEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/test").WithTags("Test");
        
        group.MapGet("/hub/connections", (ReservationHub reservationHub) => new
        {
            TotalUser = reservationHub.ConnectionManager.Connections.Keys.Count,
            TotalConnection = reservationHub.ConnectionManager.Connections.Values.SelectMany(x => x).ToList().Count,
            Data = reservationHub.ConnectionManager.Connections.Select( x => new
            {
                UserId = x.Key,
                Connections = x.Value
            })
        });
        
        group.MapGet("/hub/reservation", (ReservationGroupService reservationGroupService) 
            => reservationGroupService.Groups);
    }
}