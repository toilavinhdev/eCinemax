﻿using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries.ShowTimeQueries;

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