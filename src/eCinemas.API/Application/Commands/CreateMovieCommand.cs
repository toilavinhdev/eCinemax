using AutoMapper;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateMovieCommand : IAPIRequest
{
    public string Title { get; set; } = default!;
    
    public string? Description { get; set; }
    
    public MovieStatus Status { get; set; }
    
    public List<string>? Genres { get; set; }
     
    public List<string>? ImageUrls { get; set; }
    
    public long DurationMinutes { get; set; }
}

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.DurationMinutes).NotNull();
    }
}

public class CreateMovieCommandHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<CreateMovieCommand>
{
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    
    public async Task<APIResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var document = mapper.Map<Movie>(request);
        await _movieCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
        return APIResponse.IsSuccess();
    }
}