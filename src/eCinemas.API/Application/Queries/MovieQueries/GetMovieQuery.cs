using AutoMapper;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Application.Responses;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Queries.MovieQueries;

public class GetMovieQuery : IAPIRequest<GetMovieResponse>
{
    public string Id { get; set; } = default!;
}

public class GetMovieQueryValidator : AbstractValidator<GetMovieQuery>
{
    public GetMovieQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Vui lòng nhập Id phim");
    }
}

public class GetMovieQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetMovieQuery, GetMovieResponse>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    
    public async Task<APIResponse<GetMovieResponse>> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _movieCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Movie>.ThrowIfNotFound(movie, "Không tìm thấy phim");
        return APIResponse<GetMovieResponse>.IsSuccess(mapper.Map<GetMovieResponse>(movie));
    }
}