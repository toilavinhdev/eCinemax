using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Application.Responses;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.MovieCommands;

public class CreateReviewCommand : IAPIRequest<ReviewViewModel>
{
    public string MovieId { get; set; } = default!;
    
    public int Rate { get; set; }

    public string? Review { get; set; }
    
    internal class CreateReviewCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateReviewCommand, ReviewViewModel>
    {
        private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
        private readonly IMongoCollection<MovieReview> _reviewCollection = mongoService.Collection<MovieReview>();
        
        public async Task<APIResponse<ReviewViewModel>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieCollection
                .Find(x => x.Id == request.MovieId)
                .FirstOrDefaultAsync(cancellationToken);
            DocumentNotFoundException<Movie>.ThrowIfNotFound(movie, "Không tìm thấy phim");

            var review = await _reviewCollection
                .Find(x => x.CreatedBy == mongoService.UserClaims().Id && x.MovieId == request.MovieId)
                .FirstOrDefaultAsync(cancellationToken);
            if (review is not null) throw new BadRequestException("Phim này đã được đánh giá");
            
            var document = new MovieReview
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Rate = request.Rate,
                Review = request.Review,
                MovieId = movie.Id
            };
            document.MarkCreated(mongoService.UserClaims().Id);

            await _reviewCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

            return APIResponse<ReviewViewModel>.IsSuccess(
                new ReviewViewModel
                {
                    Id = document.Id,
                    CreatedAt = document.CreatedAt,
                    Rate = document.Rate,
                    Review = document.Review,
                    User = mongoService.UserClaims().FullName
                });
        }
    }
}

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("Vui lòng chọn phim");
        RuleFor(x => x.Rate)
            .NotNull().WithMessage("Vui lòng điền điểm đánh giá")
            .GreaterThanOrEqualTo(0).WithMessage("Điểm đánh giá phải lớn hơn hoặc bằng 0")
            .LessThanOrEqualTo(10).WithMessage("Điểm đánh giá phải nhỏ hơn hoặc bằng 10");
    }
}