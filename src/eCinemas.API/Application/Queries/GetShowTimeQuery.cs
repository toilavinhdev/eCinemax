using AutoMapper;
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
    
    public async Task<APIResponse<GetShowTimeResponse>> Handle(GetShowTimeQuery request, CancellationToken cancellationToken)
    {
        var document = await _showTimeCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(document, request.Id);

        return APIResponse<GetShowTimeResponse>.IsSuccess(mapper.Map<GetShowTimeResponse>(document));
    }
}