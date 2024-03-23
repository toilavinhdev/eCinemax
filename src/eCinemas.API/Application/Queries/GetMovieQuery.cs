using AutoMapper;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries;

public class GetMovieQuery : IAPIRequest<GetMovieResponse>
{
    public string Id { get; set; } = default!;
}

public class GetMovieQueryValidator : AbstractValidator<GetMovieQuery>
{
    public GetMovieQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetMovieQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetMovieQuery>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    
    public async Task<APIResponse> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Movie>.ThrowIfNotFound(movie);

        var data = mapper.Map<GetMovieResponse>(movie);

        return APIResponse<GetMovieResponse>.IsSuccess(data);
    }
}