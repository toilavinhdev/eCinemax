using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.UserQueries;

public class GetMeQuery : IAPIRequest<GetMeResponse>
{
    
}

public class GetMeQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetMeQuery, GetMeResponse>
{
    private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
    
    public async Task<APIResponse<GetMeResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = mongoService.UserClaims()?.Id;
        if (userId is null) throw new NullReferenceException("UserId is required");

        var user = await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken);
        if (user is null) throw new BadRequestException("Người dùng không tồn tại");
        
        return APIResponse<GetMeResponse>.IsSuccess(mapper.Map<GetMeResponse>(user));
    }
}