using eCinemas.API.Shared.ValueObjects;
using MediatR;

namespace eCinemas.API.Shared.Mediator;

public interface IAPIRequest : IRequest<APIResponse>
{
    
}

public interface IAPIRequest<TResponse> : IRequest<APIResponse<TResponse>>
{
    
}