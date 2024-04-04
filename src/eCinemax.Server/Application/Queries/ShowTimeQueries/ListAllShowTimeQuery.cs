using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.ShowTimeQueries;

public class ListAllShowTimeQuery : IAPIRequest<List<ShowTime>> //for admin
{
}

public class ListAllShowTimeQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListAllShowTimeQuery, List<ShowTime>>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task<APIResponse<List<ShowTime>>> Handle(ListAllShowTimeQuery request, CancellationToken cancellationToken)
    {
        return APIResponse<List<ShowTime>>.IsSuccess(
            await _showTimeCollection
                .Find(Builders<ShowTime>.Filter.Empty)
                .ToListAsync(cancellationToken));
    }
}