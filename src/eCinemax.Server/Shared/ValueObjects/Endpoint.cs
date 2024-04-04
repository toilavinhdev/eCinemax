namespace eCinemax.Server.Shared.ValueObjects;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}