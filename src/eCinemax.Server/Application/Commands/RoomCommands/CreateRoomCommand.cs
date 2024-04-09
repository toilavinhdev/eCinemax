using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Aggregates.RoomAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Extensions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.RoomCommands;

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
            SeatCount = seats
                .SelectMany(x => x)
                .Count(x => x.Type != SeatType.Blank)
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