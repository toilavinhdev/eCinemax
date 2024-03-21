using eCinemas.API.ValueObjects;
using MediatR;

namespace eCinemas.API.Mediator;

public interface IAPIRequest : IRequest<APIResponse>
{
    
}

public interface IAPIRequest<TResponse> : IRequest<APIResponse<TResponse>>
{
    
}