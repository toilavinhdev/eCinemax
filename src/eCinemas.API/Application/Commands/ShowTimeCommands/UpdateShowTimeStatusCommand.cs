using eCinemas.API.Aggregates.ShowtimeAggregate;
using eCinemas.API.Infrastructure.Persistence;
using MediatR;
using MongoDB.Driver;

namespace eCinemas.API.Application.Commands.ShowTimeCommands;

public class UpdateShowTimeStatusCommand : IRequest;

public class UpdateShowTimeStatusCommandHandler(IMongoService mongoService) : IRequestHandler<UpdateShowTimeStatusCommand>
{
    private readonly IMongoCollection<ShowTime> _showTimeCollection = mongoService.Collection<ShowTime>();
    
    public async Task Handle(UpdateShowTimeStatusCommand request, CancellationToken cancellationToken)
    {
        // TODO: Cập nhật trạng thái lịch chiếu theo thời gian
        
        var filterBuilder = Builders<ShowTime>.Filter;
        var filter = filterBuilder.Empty;
        filter &= filterBuilder.Eq(x => x.Status, ShowTimeStatus.Upcoming);
        
        var cursor = await _showTimeCollection.FindAsync(filter, cancellationToken: cancellationToken);
        
        while (await cursor.MoveNextAsync(cancellationToken))
        {
            var batch = cursor.Current;
            foreach (var document in batch)
            {
                if (document.StartAt > DateTime.Now) continue;

                var status = (document.StartAt <= DateTime.Now && document.FinishAt > DateTime.Now)
                    ? ShowTimeStatus.NowShowing
                    : ShowTimeStatus.Finished;
                
                var update = Builders<ShowTime>.Update.Set(x => x.Status, status);
                var filterUpdate = Builders<ShowTime>.Filter.Eq(x => x.Id, document.Id);
                await _showTimeCollection.UpdateOneAsync(
                    filterUpdate, 
                    update,
                    cancellationToken: cancellationToken);
            }
        }
    }
}