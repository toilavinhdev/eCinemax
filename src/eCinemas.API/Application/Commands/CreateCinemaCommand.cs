using AutoMapper;
using eCinemas.API.Aggregates.CinemaAggregate;
using eCinemas.API.Services;
using eCinemas.API.Shared.Mediator;
using eCinemas.API.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands;

public class CreateCinemaCommand : IAPIRequest<Cinema>
{
    public string Name { get; set; } = default!;

    public string Address { get; set; } = default!;
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
        document.MarkCreated();
        document.Rooms = [];
        await _cinemaCollection.InsertOneAsync(document, cancellationToken:cancellationToken);
        return APIResponse<Cinema>.IsSuccess(document);
    }
}