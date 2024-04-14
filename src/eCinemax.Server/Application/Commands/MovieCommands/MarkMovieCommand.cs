using eCinemax.Server.Aggregates.MovieAggregate;
using eCinemax.Server.Infrastructure.Persistence;
using eCinemax.Server.Shared.Exceptions;
using eCinemax.Server.Shared.Mediator;
using eCinemax.Server.Shared.ValueObjects;
using FluentValidation;
using MongoDB.Driver;

namespace eCinemax.Server.Application.Commands.MovieCommands;

public class MarkMovieCommand : IAPIRequest
{
    public List<string> Ids { get; set; } = default!;
    
    public bool IsMark { get; set; }
    
    internal class MarkMovieCommandHandler(IMongoService mongoService) : IAPIRequestHandler<MarkMovieCommand>
    {
        private readonly IMongoCollection<Movie> _movieCollection = mongoService.Collection<Movie>();
        
        public async Task<APIResponse> Handle(MarkMovieCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = mongoService.UserClaims().Id;
            
            foreach (var movieId in request.Ids)
            {
                var builder = Builders<Movie>.Filter;
                var filter = builder.Empty;
                filter &= builder.Eq(x => x.Id, movieId);

                var movie = await _movieCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
                
                if (movie is null) 
                    throw new DocumentNotFoundException<Movie>("Có lỗi xảy ra. Vui lòng thử lại");

                // TODO: Cần optimize array field cho số lượng record lớn
                var existed = movie.UserMarks!.Any(x => x == currentUserId);

                if (request.IsMark)
                {
                    if (movie.UserMarks is null)
                    {
                        movie.UserMarks =  [ currentUserId ];
                    }
                    else
                    {
                        movie.UserMarks.Add(currentUserId);
                    }
                }
                else
                {
                    movie.UserMarks?.Remove(currentUserId);
                }

                var update = Builders<Movie>.Update.Set(x => x.UserMarks, movie.UserMarks);
                await _movieCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            }

            return APIResponse.IsSuccess();
        }
    }
}

public class MarkMovieCommandValidator : AbstractValidator<MarkMovieCommand>
{
    public MarkMovieCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}