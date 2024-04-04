using eCinemax.Server.Shared.ValueObjects;
using MediatR;

namespace eCinemax.Server.Shared.Mediator;

public interface IAPIRequest : IRequest<APIResponse>
{
    
}

public interface IAPIRequest<TResponse> : IRequest<APIResponse<TResponse>>
{
    
}