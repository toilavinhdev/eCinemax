using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.MovieCommands;

public class CreateMovieRateCommand : IAPIRequest<MovieRate>
{
    public string MovieId { get; set; } = default!;
    
    public int Rate { get; set; }
    
    public string? Comment { get; set; }
    
    internal class CreateMovieRateCommandHandler(IMongoService mongoService) : IAPIRequestHandler<CreateMovieRateCommand, MovieRate>
    {
        private readonly IMongoCollection<MovieRate> _rateCollection = mongoService.Collection<MovieRate>();
        
        public async Task<APIResponse<MovieRate>> Handle(CreateMovieRateCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = mongoService.UserClaims().Id;

            var existed = await _rateCollection
                .Find(x => x.MovieId == request.MovieId && x.CreatedBy == currentUserId)
                .AnyAsync(cancellationToken);

            if (!existed) throw new BadRequestException("Phim này đã được đánh giá");

            var document = new MovieRate
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Rate = request.Rate,
                Comment = request.Comment,
            };
            document.MarkCreated(currentUserId);

            await _rateCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

            return APIResponse<MovieRate>.IsSuccess(document);
        }
    }
}

public class CreateMovieRateCommandValidator : AbstractValidator<CreateMovieRateCommand>
{
    public CreateMovieRateCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("Vui lòng chọn phim");
        RuleFor(x => x.Rate)
            .NotEmpty().WithMessage("Vui lòng đánh giá phim")
            .GreaterThan(0).WithMessage("Điểm đánh giá phải lớn hơn hoặc bằng 0")
            .LessThanOrEqualTo(10).WithMessage("Điểm đánh giá phải nhỏ hơn hoặc bằng 10");
    }
}