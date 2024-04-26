using eCinemax.Server.Aggregates.CinemaAggregate;
using eCinemax.Server.Aggregates.ShowtimeAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Persistence;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.ShowTimeQueries;

public class GetShowTimeQuery : IAPIRequest<GetShowTimeResponse>
{
    public string Id { get; set; } = default!;
}

public class GetShowTimeQueryValidator : AbstractValidator<GetShowTimeQuery>
{
    public GetShowTimeQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetShowTimeQueryHandler(IMongoService mongoService, IMapper mapper) : IAPIRequestHandler<GetShowTimeQuery, GetShowTimeResponse>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    private readonly IMongoCollection<Cinema> _cinemaCollection = mongoService.Collection<Cinema>();
    
    public async Task<APIResponse<GetShowTimeResponse>> Handle(GetShowTimeQuery request, CancellationToken cancellationToken)
    {
        var document = await _showTimeCollection
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<ShowTime>.ThrowIfNotFound(document, "Không tìm thấy lịch chiếu");

        var cinema = await _cinemaCollection
            .Find(x => x.Id == document.CinemaId)
            .FirstOrDefaultAsync(cancellationToken);
        DocumentNotFoundException<Cinema>.ThrowIfNotFound(cinema, "Không tìm thấy rạp phim");

        if (document.Status == ShowTimeStatus.Finished) throw new BadRequestException("Lịch chiếu đã kết thúc");

        var response = mapper.Map<GetShowTimeResponse>(document);
        response.CinemaName = cinema.Name;

        return APIResponse<GetShowTimeResponse>.IsSuccess(response);
    }
}