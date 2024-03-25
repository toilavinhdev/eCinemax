using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateRoomCommand : IAPIRequest<Room>
{
    public string CinemaId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public SeatType[][] SeatsTypes { get; set; } = default!;
}

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.CinemaId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.SeatsTypes).NotEmpty();
    }
}

public class CreateRoomCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateRoomCommand, Room>
{
    private readonly IMongoCollection<Room> _roomCollection = mongoService.Collection<Room>();
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<Room>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var cinema = await _cinemaCollection
            .Find(x => x.Id == request.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Cinema>.ThrowIfNotFound(cinema, request.CinemaId);

        var document = new Room
        {
            Cinema = request.CinemaId,
            Name = request.Name,
        };
        
        // handle seats 2d array
        var rowCount = request.SeatsTypes.Length;
        var seats = new Seat[rowCount][];

        for (var row = 0; row < rowCount; row++)
        {
            var columnCount = request.SeatsTypes[row].Length;
            seats[row] = new Seat[columnCount];

            for (var column = 0; column < columnCount; column++)
            {
                seats[row][column] = new Seat
                {
                    Row = row.ToSeatCharacter().ToString(),
                    Column = column + 1,
                    Type = request.SeatsTypes[row][column]
                };
            }
        }

        document.Seats = seats;
        document.MarkCreated();

        await _roomCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

        return APIResponse<Room>.IsSuccess(document);
    }
}