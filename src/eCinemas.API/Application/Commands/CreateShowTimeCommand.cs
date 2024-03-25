using AutoMapper;
using eCinemas.API.Aggregates.MovieAggregate;
using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateShowTimeCommand : IAPIRequest<ShowTime>
{
    public string MovieId { get; set; } = default!;

    public string RoomId { get; set; } = default!;

    public string StartAt { get; set; } = default!; // 2024-03-25 10:30:00;

    public List<SeatPrice> SeatPrices { get; set; } = default!;
}

public class CreateShowTimeCommandValidator : AbstractValidator<CreateShowTimeCommand>
{
    public CreateShowTimeCommandValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.RoomId).NotEmpty();
        RuleFor(x => x.StartAt).NotEmpty();
        RuleFor(x => x.SeatPrices).NotEmpty();
    }
}

public class CreateShowTimeCommandHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<CreateShowTimeCommand, ShowTime>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
    private readonly IMongoCollection<Room> _roomCollection = mongoService.Collection<Room>();
    
    public async Task<APIResponse<ShowTime>> Handle(CreateShowTimeCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieCollection
            .Find(x => x.Id == request.MovieId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Movie>.ThrowIfNotFound(movie, request.MovieId);
        var room = await _roomCollection
            .Find(x => x.Id == request.RoomId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Room>.ThrowIfNotFound(room, request.RoomId);

        var document = mapper.Map<ShowTime>(request);
        document.StartTime = DateTime.Parse(request.StartAt);
        document.MarkCreated();
        await _showTimeCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

        return APIResponse<ShowTime>.IsSuccess(document);
    }
}