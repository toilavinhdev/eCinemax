using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.RoomQueries;

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