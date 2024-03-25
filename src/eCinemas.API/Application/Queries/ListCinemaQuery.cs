using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class ListCinemaQuery : IAPIRequest<List<CinemaViewList>>, IPaginationRequest
{
    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 15;
}

public class ListCinemaQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListCinemaQuery, List<CinemaViewList>>
{
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<List<CinemaViewList>>> Handle(ListCinemaQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<Cinema>.Filter.Empty;
        var documents = await _cinemaCollection
            .Find(filter)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Limit(request.PageSize)
            .Project(x => new CinemaViewList
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address
            })
            .ToListAsync(cancellationToken);
        return APIResponse<List<CinemaViewList>>.IsSuccess(documents.ToList());
    }
}