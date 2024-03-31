using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Services;
using MediatR;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.ShowTimeCommands;

public class UpdateShowTimeStatusCommand : IRequest
{
    
}

public class UpdateShowTimeStatusCommandHandler(IMongoService mongoService) : IRequestHandler<UpdateShowTimeStatusCommand>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task Handle(UpdateShowTimeStatusCommand request, CancellationToken cancellationToken)
    {
        var filterBuilder = Builders<ShowTime>.Filter;
        var filter = filterBuilder.Empty;
        filter &= filterBuilder.Eq(x => x.Status, ShowTimeStatus.Upcoming);
        var cursor = await _showTimeCollection.FindAsync(filter, cancellationToken: cancellationToken);
        
        while (await cursor.MoveNextAsync(cancellationToken))
        {
            var batch = cursor.Current;
            foreach (var document in batch)
            {
                if (document.StartAt >= DateTime.Now) continue;
                var update = Builders<ShowTime>.Update.Set(x => x.Status, ShowTimeStatus.Finished);
                var filterUpdate = Builders<ShowTime>.Filter.Eq(x => x.Id, document.Id);
                await _showTimeCollection.UpdateOneAsync(
                    filterUpdate, 
                    update,
                    cancellationToken: cancellationToken);
            }
        }
    }
}