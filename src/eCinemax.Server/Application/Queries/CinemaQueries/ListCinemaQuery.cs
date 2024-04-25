using eCinemax.Server.Aggregates.CinemaAggregate;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.CinemaQueries;

public class ListCinemaQuery : IAPIRequest<List<Cinema>>, IPaginationRequest
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 15;
}

public class ListCinemaQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListCinemaQuery, List<Cinema>>
{
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<List<Cinema>>> Handle(ListCinemaQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Cinema>.Filter.Empty;
        var documents = await _cinemaCollection
            .Find(filter)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync(cancellationToken);
        return APIResponse<List<Cinema>>.IsSuccess(documents);
    }
}