using AutoMapper;
using eCinemas.API.Aggregates.UserAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class GetMeQuery : IAPIRequest<GetMeResponse>
{
    
}

public class GetMeQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetMeQuery, GetMeResponse>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = mongoService.GetUserClaimValue()?.Id;
        if (userId is null) throw new NullReferenceException("UserId is required");

        var user = await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new BadRequestException("Người dùng không tồn tại");
        
        return APIResponse<GetMeResponse>.IsSuccess(mapper.Map<GetMeResponse>(user));
    }
}