using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Aggregates.RoomAggregate;
using eCinemas.API.Infrastructure.Persistence;
using eCinemas.API.Shared.Exceptions;
using eCinemas.API.Shared.Extensions;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.RoomCommands;

public class CreateRoomCommand : IAPIRequest<Room>
{
    public string CinemaId { get; set; } = default!;

    public string Name { get; set; } = default!;

    public List<List<SeatType>> SeatsTypes { get; set; } = default!; // Row<Column<Type>>
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
        var existedCinema = await _cinemaCollection
            .Find(x => x.Id == request.CinemaId)
            .AnyAsync(cancellationToken);
        if (!existedCinema) throw new DocumentNotFoundException<Cinema>(request.CinemaId);
        
        // handle seats 2d
        var seats = request.SeatsTypes
            .Select((columnTypes, rowIndex) =>
                columnTypes.Select((columnType, columnIndex) => new Seat
                {
                    Row = rowIndex.ToSeatCharacter().ToString(),
                    Column = columnIndex + 1,
                    Type = columnType
                }).ToList())
            .ToList();

        var document = new Room
        {
            CinemaId = request.CinemaId,
            Name = request.Name,
            Seats = seats,
            SeatCount = seats.Count
        };
        document.MarkCreated();
        
        await _roomCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

        await _cinemaCollection.UpdateOneAsync(
            x => x.Id == request.CinemaId,
            Builders<Cinema>.Update.Push(x => x.RoomIds, document.Id),
            cancellationToken: cancellationToken);
        
        return APIResponse<Room>.IsSuccess(document);
    }
}