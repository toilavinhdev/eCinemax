using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class ListRoomQuery : IAPIRequest<List<Room>>
{
    
}

public class ListRoomQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListRoomQuery, List<Room>>
{
    private readonly IMongoCollection<Room> _roomCollection = mongoService.Collection<Room>();
    
    public async Task<APIResponse<List<Room>>> Handle(ListRoomQuery request, CancellationToken cancellationToken)
    {
        var data = await _roomCollection.Find(Builders<Room>.Filter.Empty).ToListAsync(cancellationToken);
        return APIResponse<List<Room>>.IsSuccess(data);
    }
}