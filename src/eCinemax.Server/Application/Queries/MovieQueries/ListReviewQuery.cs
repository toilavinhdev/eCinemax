using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Aggregates.UserAggregate;
using eCinemax.Server.Application.Responses;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Queries.MovieQueries;

public class ListReviewQuery : IAPIRequest<ListReviewResponse>
{
    public int PageIndex { get; set; }
    
    public int PageSize { get; set; }

    public string MovieId { get; set; } = default!; 
    
    internal class ListReviewQueryHandler(IMongoService mongoService) : IAPIRequestHandler<ListReviewQuery, ListReviewResponse>
    {
        private readonly IMongoCollection<MovieReview> _reviewCollection = mongoService.Collection<MovieReview>();
        private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
        private readonly IMongoCollection<User> _userCollection = mongoService.Collection<User>();
        
        public async Task<APIResponse<ListReviewResponse>> Handle(ListReviewQuery request, CancellationToken cancellationToken)
        {
            var totalRecord = await _reviewCollection
                .Find(Builders<MovieReview>.Filter.Empty)
                .CountDocumentsAsync(cancellationToken);

            var documents = await _reviewCollection
                .Aggregate()
                .Match(x => x.MovieId == request.MovieId)
                .SortBy(x => x.CreatedAt)
                .Skip(request.PageSize * (request.PageIndex - 1))
                .Limit(request.PageSize)
                .Lookup<MovieReview, User, MovieReviewWithUser>(
                    _userCollection,
                    l => l.CreatedBy,
                    f => f.Id,
                    review => review.Users)
                .Project(x => new ReviewViewModel
                {
                    Id = x.Id,
                    User = x.Users.FirstOrDefault()!.FullName,
                    Rate = x.Rate,
                    Review = x.Review,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return APIResponse<ListReviewResponse>.IsSuccess(new ListReviewResponse(
                documents,
                request.PageIndex,
                request.PageSize,
                (int)totalRecord));
        }
    }
    
    private class MovieReviewWithUser : MovieReview
    {
        public List<User> Users { get; set; } = default!;
    }
}

public class ListReviewQueryValidator : AbstractValidator<ListReviewQuery>
{
    public ListReviewQueryValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0);
        RuleFor(x => x.MovieId).NotEmpty();
    }
}