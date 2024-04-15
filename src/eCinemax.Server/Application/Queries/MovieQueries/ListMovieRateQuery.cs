using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.MovieQueries;

public class ListMovieRateQuery : IAPIRequest<ListMovieRateResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }

    internal class ListMovieRateQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListMovieRateQuery, ListMovieRateResponse>
    {
        private readonly IMongoCollection<MovieRate> _rateCollection = mongoService.Collection<MovieRate>();
        
        public async Task<APIResponse<ListMovieRateResponse>> Handle(ListMovieRateQuery request, CancellationToken cancellationToken)
        {
            var builder = Builders<MovieRate>.Filter;
            var filter = builder.Empty;

            var fluent = _rateCollection.Find(filter);

            var totalRecord = await fluent.CountDocumentsAsync(cancellationToken);

            // var data = await fluent
            //     .SortByDescending(x => x.CreatedAt)
            //     .Skip(request.PageSize * (request.PageIndex - 1))
            //     .Limit(request.PageSize)
            //     .ToListAsync(cancellationToken);

            // var data = _rateCollection
            //     .Aggregate()
            //     .Lookup()

            // return APIResponse<ListMovieRateResponse>.IsSuccess(
            //     new ListMovieRateResponse(
            //         data, 
            //         request.PageIndex,
            //         request.PageSize, 
            //         (int)totalRecord));

            throw new NotImplementedException();
        }
    }
}

public class ListMovieRateQueryValidator : AbstractValidator<ListMovieRateQuery>
{
    public ListMovieRateQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
    }
}