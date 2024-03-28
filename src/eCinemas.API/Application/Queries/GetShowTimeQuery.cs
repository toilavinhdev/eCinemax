using AutoMapper;
using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class GetShowTimeQuery : IAPIRequest<GetShowTimeResponse>
{
    public string Id { get; set; } = default!;
}

public class GetShowTimeQueryValidator : AbstractValidator<GetShowTimeQuery>
{
    public GetShowTimeQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetShowTimeQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetShowTimeQuery, GetShowTimeResponse>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<GetShowTimeResponse>> Handle(GetShowTimeQuery request, CancellationToken cancellationToken)
    {
        var document = await _showTimeCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(document, request.Id);

        var cinema = await _cinemaCollection
            .Find(x => x.Id == document.Cinema)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Cinema>.ThrowIfNotFound(cinema, document.Cinema);

        var response = mapper.Map<GetShowTimeResponse>(document);
        response.CinemaName = cinema.Name;

        return APIResponse<GetShowTimeResponse>.IsSuccess(response);
    }
}