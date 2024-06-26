﻿using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.CinemaCommands;

public class CreateCinemaCommand : IAPIRequest<Cinema>
{
    public string Name { get; set; } = default!;

    public string Address { get; set; } = default!;
    
    public CinemaLocation? Location { get; set; } 
}

public class CreateCinemaCommandValidator : AbstractValidator<CreateCinemaCommand>
{
    public CreateCinemaCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();
    }
}

public class CreateCinemaCommandHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<CreateCinemaCommand, Cinema>
{
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<Cinema>> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
    {
        var document = mapper.Map<Cinema>(request);
        document.RoomIds = [];
        await _cinemaCollection.InsertOneAsync(document, cancellationToken:cancellationToken);
        return APIResponse<Cinema>.IsSuccess(document);
    }
}