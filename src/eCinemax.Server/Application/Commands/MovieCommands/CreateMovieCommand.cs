using AutoMapper;
using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.MovieCommands;

public class CreateMovieCommand : IAPIRequest<Movie>
{
    public string Title { get; set; } = default!;

    public string Plot { get; set; } = default!;

    public List<string> Directors { get; set; } = default!;

    public List<string> Casts { get; set; } = default!;
    
    public int Age { get; set; } = default!;

    public List<string> Languages { get; set; } = default!;
    
    public MovieStatus Status { get; set; }

    public List<string> Genres { get; set; } = default!;

    public string PosterUrl { get; set; } = default!;
    
    public DateTime? ReleasedAt { get; set; }
    
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
        RuleFor(x => x.Age).NotNull().GreaterThanOrEqualTo(0);
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
        document.UserMarks = [];
        document.MarkCreated();
        await _movieCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return APIResponse<Movie>.IsSuccess(document);
    }
}