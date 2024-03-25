using AutoMapper;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateMovieCommand : IAPIRequest<Movie>
{
    public string Title { get; set; } = default!;

    public string Plot { get; set; } = default!;

    public List<string> Directors { get; set; } = default!;

    public List<string> Casts { get; set; } = default!;

    public List<string> Languages { get; set; } = default!;
    
    public MovieStatus Status { get; set; }

    public List<string> Genres { get; set; } = default!;

    public string PosterUrl { get; set; } = default!;
    
    public string? ReleasedAt { get; set; }
    
    public long DurationMinutes { get; set; }
}

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Plot).NotEmpty();
        RuleFor(x => x.Directors).NotEmpty();
        RuleFor(x => x.Casts).NotNull();
        RuleFor(x => x.Languages).NotEmpty();
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.Genres).NotNull();
        RuleFor(x => x.PosterUrl).NotEmpty();
        RuleFor(x => x.DurationMinutes).NotNull();
    }
}

public class CreateMovieCommandHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<CreateMovieCommand, Movie>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    
    public async Task<APIResponse<Movie>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var document = mapper.Map<Movie>(request);
        document.Released = request.ReleasedAt is not null ? DateOnly.Parse(request.ReleasedAt) : null;
        document.MarkCreated();
        await _movieCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return APIResponse<Movie>.IsSuccess(document);
    }
}