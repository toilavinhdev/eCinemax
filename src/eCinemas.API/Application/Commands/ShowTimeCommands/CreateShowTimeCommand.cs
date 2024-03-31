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

namespace eCinemas.API.Application.Commands.ShowTimeCommands;

public class CreateShowTimeCommand : IAPIRequest<ShowTime>
{
    public string Movie { get; set; } = default!;

    public string Room { get; set; } = default!;

    public DateTime StartAt { get; set; } = default!; //iso 8601 -> datetime utc

    public List<SeatPrice> Ticket { get; set; } = default!;
}

public class CreateShowTimeCommandValidator : AbstractValidator<CreateShowTimeCommand>
{
    public CreateShowTimeCommandValidator()
    {
        RuleFor(x => x.Movie).NotEmpty();
        RuleFor(x => x.Room).NotEmpty();
        RuleFor(x => x.StartAt).NotEmpty();
        RuleFor(x => x.Ticket).NotEmpty();
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
            .Find(x => x.Id == request.Movie)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Movie>.ThrowIfNotFound(movie, "Không tìm thấy phim");
        var room = await _roomCollection
            .Find(x => x.Id == request.Room)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Room>.ThrowIfNotFound(room, "Không tìm thấy phòng chiếu");

        var document = mapper.Map<ShowTime>(request);
        document.MovieId = movie.Id;
        document.RoomId = room.Id;
        document.CinemaId = room.CinemaId;
        document.Available = room.Seats.Aggregate(0, (acc, cur) => acc + cur.Count(x => x.Type != SeatType.Blank));
        document.Reservations = room.Seats
            .Select(rowSeats => 
                rowSeats.Select(seat => new Reservation
                {
                    Row = seat.Row,
                    Column = seat.Column,
                    Type = seat.Type,
                    Status = ReservationStatus.Idle,
                }).ToList()
            ).ToList();
        document.MarkCreated();
        await _showTimeCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

        return APIResponse<ShowTime>.IsSuccess(document);
    }
}